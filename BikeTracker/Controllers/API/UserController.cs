using BikeTracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;

namespace BikeTracker.Controllers.API
{
    [Authorize(Roles = "GeneralAdmin")]
    public class UserController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: odata/User
        [EnableQuery]
        public IQueryable<UserAdminModel> GetUser()
        {
            return db.Users.Select(u => new UserAdminModel
            {
                Id = u.Id,
                EmailAddress = u.Email,
                Role = db.Roles.FirstOrDefault(r => r.Id == u.Roles.FirstOrDefault().RoleId).DisplayName,
                RoleId = u.Roles.FirstOrDefault().RoleId,
                UserName = u.Email
            });
        }

        // GET: odata/User(5)
        [EnableQuery]
        public SingleResult<UserAdminModel> GetApplicationUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.Users.Where(applicationUser => applicationUser.Id == key).Select(u => new UserAdminModel
            {
                Id = u.Id,
                EmailAddress = u.Email,
                Role = db.Roles.FirstOrDefault(r => r.Id == u.Roles.FirstOrDefault().RoleId).DisplayName,
                RoleId = u.Roles.FirstOrDefault().RoleId,
                UserName = u.Email
            }));
        }

        // PUT: odata/User(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, UserAdminModel update)
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

            if (user.Email != update.EmailAddress)
            {
                await UserManager.SetEmailAsync(key, update.EmailAddress);
                await UserManager.GenerateEmailConfirmationEmailAsync(new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext), key);
            }

            if (!await UserManager.IsInRoleAsync(key, update.Role))
            {
                var roles = await UserManager.GetRolesAsync(key);
                await UserManager.RemoveFromRolesAsync(key, roles.ToArray());
                await UserManager.AddToRoleAsync(key, update.Role);
            }

            return Ok();
        }

        // POST: odata/User
        public async Task<IHttpActionResult> Post(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var id = (await UserManager.FindByEmailAsync(model.Email)).Id;
                await UserManager.AddToRoleAsync(user.Id, model.Role);

                await UserManager.GenerateEmailConfirmationEmailAsync(new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext), id);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        // DELETE: odata/User(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            var user = await UserManager.FindByIdAsync(key);
            if (user != null)
                await UserManager.DeleteAsync(user);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
