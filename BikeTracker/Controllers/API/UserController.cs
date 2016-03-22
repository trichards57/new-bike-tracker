using BikeTracker.Controllers.Filters;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
using System.Web.Security;

namespace BikeTracker.Controllers.API
{
    [Authorize(Roles = "GeneralAdmin")]
    public class UserController : ODataController
    {
        private ILogService logService;
        private IRoleManager roleManager;
        private IUserManager userManager;

        [InjectionConstructor, IgnoreCoverage]
        public UserController() { }

        public UserController(IUserManager userManager, IRoleManager roleManager, ILogService logService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logService = logService;
        }

        public IRoleManager RoleManager
        {
            get
            {
                return roleManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        public IUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // DELETE: odata/User(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            var user = await UserManager.FindByIdAsync(key);
            if (user != null)
            {
                await UserManager.DeleteAsync(user);
                await logService.LogUserDeleted(User.Identity.GetUserName(), user.UserName);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/User(5)
        [EnableQuery]
        public async Task<SingleResult<UserAdminViewModel>> Get([FromODataUri] string key)
        {
            var user = await UserManager.FindByIdAsync(key);
            if (user != null)
            {
                var roles = await UserManager.GetRolesAsync(key);
                var role = roles.FirstOrDefault();
                var roleDetails = await RoleManager.FindByNameAsync(role);

                return SingleResult.Create(new List<UserAdminViewModel>
                {
                    new UserAdminViewModel
                    {
                        Id = user.Id,
                        EmailAddress = user.Email,
                        Role = roleDetails.Name,
                        RoleDisplayName = roleDetails.DisplayName,
                        RoleId = roleDetails.Id,
                        UserName = user.UserName
                    }
                }.AsQueryable());
            }

            return SingleResult.Create(new List<UserAdminViewModel>().AsQueryable());
        }

        // GET: odata/User
        [EnableQuery]
        public async Task<IQueryable<UserAdminViewModel>> GetUser()
        {
            var users = await UserManager.Users.ToListAsync();

            var userRoleTask = users.Select(async user =>
            {
                var roles = await UserManager.GetRolesAsync(user.Id);
                var role = roles.FirstOrDefault();
                var roleDetails = await RoleManager.FindByNameAsync(role);
                return new { user, role = roleDetails };
            });

            var userRole = await Task.WhenAll(userRoleTask);

            return userRole.Select(d => new UserAdminViewModel
            {
                Id = d.user.Id,
                EmailAddress = d.user.Email,
                Role = d.role.Name,
                RoleDisplayName = d.role.DisplayName,
                RoleId = d.role.Id,
                UserName = d.user.UserName
            }).AsQueryable();
        }

        // PUT: odata/User(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, UserAdminViewModel update)
        {
            Validate(update);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await UserManager.FindByIdAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            var changes = new List<string>();

            if (user.Email != update.EmailAddress)
            {
                await UserManager.SetEmailAsync(key, update.EmailAddress);
                await UserManager.GenerateEmailConfirmationEmailAsync(new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext), key);
                changes.Add("Email");
            }

            if (!await UserManager.IsInRoleAsync(key, update.Role))
            {
                var roles = await UserManager.GetRolesAsync(key);
                await UserManager.RemoveFromRolesAsync(key, roles.ToArray());
                await UserManager.AddToRoleAsync(key, update.Role);
                changes.Add("Role");
            }

            if (changes.Count > 0)
                await logService.LogUserUpdated(User.Identity.GetUserName(), changes);

            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Register(ODataActionParameters parameters)
        {
            var role = (string)parameters["role"];
            var email = (string)parameters["email"];

            const string DefaultRole = "Normal";

            if (string.IsNullOrEmpty(role))
                role = DefaultRole;

            if (string.IsNullOrEmpty(email))
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("email", "email must be provided");
                return BadRequest(modelState);
            }

            var addr = new EmailAddressAttribute();
            if (!addr.IsValid(email))
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("email", "email is not valid");
                return BadRequest(modelState);
            }

            if (!(await RoleManager.RoleExistsAsync(role)))
                role = DefaultRole;

            var user = new ApplicationUser { UserName = email, Email = email, MustResetPassword = true };

            string password;

            do
            {
                password = Membership.GeneratePassword(Math.Min(ApplicationUserManager.MinPasswordLength * 2, 128), 2);
            }
            while (!(await UserManager.PasswordValidator.ValidateAsync(password)).Succeeded);

            var result = await UserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var id = (await UserManager.FindByEmailAsync(email)).Id;
                await UserManager.AddToRoleAsync(id, role);

                await UserManager.GenerateEmailConfirmationEmailAsync(new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext), id, password);

                await logService.LogUserCreated(User.Identity.GetUserName(), user.UserName);

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}