using BikeTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// Controller that handles basic admin tasks.
    /// </summary>
    [Authorize(Roles = "GeneralAdmin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// The database context for this controller.
        /// </summary>
        private ApplicationDbContext dbContext = new ApplicationDbContext();

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
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        /// <summary>
        /// Begins the create user process.
        /// </summary>
        /// <returns>The Create view</returns>
        /// @mapping GET /Admin/Create
        /// @notanon
        /// @role{GeneralAdmin}
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Continues the create user process using the provided user details.
        /// </summary>
        /// <param name="model">The model containing the user details.</param>
        /// <returns>If successul, the UserAdded view, otherwise returns to the Create view.</returns>
        /// <remarks>
        /// Sets up the user and triggers the email that asks the user to confirm their
        /// email address.
        /// </remarks>
        /// @mapping POST /Admin/Create
        /// @notanon
        /// @role{GeneralAdmin}
        /// @antiforgery
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var id = UserManager.FindByEmail(model.Email).Id;
                    UserManager.AddToRole(user.Id, "Normal");

                    await UserManager.GenerateEmailConfirmationEmailAsync(Url, id);

                    return View("UserAdded", user);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Deletes the user specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The user identifier to delete.</param>
        /// <param name="collection">The details from the rest of the form.</param>
        /// <returns>If the user exists, returns a redirect to the Index, otherwise a File Not Found error.</returns>
        /// @mapping POST /Admin/Delete
        /// @notanon
        /// @role{GeneralAdmin}
        /// @antiforgery
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(string id, FormCollection collection)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                return HttpNotFound();

            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Begins the user delete process.
        /// </summary>
        /// <param name="id">The user identifier to delete.</param>
        /// <returns>If the user exists, the Delete view, otherwise File Not Found</returns>
        /// @mapping GET /Admin/Delete
        /// @notanon
        /// @role{GeneralAdmin}
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        /// <summary>
        /// Displays the root User Admin console
        /// </summary>
        /// <returns>The Index view which displays the given user.</returns>
        /// @mapping GET /Admin/
        /// @mapping GET /Admin/Index
        /// @notanon
        /// @role{GeneralAdmin}
        public ActionResult Index()
        {
            var users = dbContext.Users.Select(u => new UserAdminModel
            {
                Id = u.Id,
                UserName = u.UserName,
                EmailAddress = u.Email,
                Role = dbContext.Roles.FirstOrDefault(r => r.Id == u.Roles.FirstOrDefault().RoleId).DisplayName
            });

            return View(users);
        }

        /// <summary>
        /// Displays the user update screen.
        /// </summary>
        /// <param name="id">The identifier of the user to update.</param>
        /// <returns>The Update view if the user exists, otherwise File Not Found.</returns>
        /// @mapping GET /Admin/Update
        /// @notanon
        /// @role{GeneralAdmin}
        [HttpGet]
        public ActionResult Update(string id)
        {
            var model = dbContext.Users.Select(u => new UserAdminModel
            {
                Id = u.Id,
                EmailAddress = u.Email,
                UserName = u.UserName,
                RoleId = dbContext.Roles.FirstOrDefault(r => r.Id == u.Roles.FirstOrDefault().RoleId).Id,
                Roles = dbContext.Roles.Select(r => new SelectListItem { Text = r.DisplayName, Value = r.Id }),
            }).FirstOrDefault(u => u.Id == id);

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        /// <summary>
        /// Updates the specified user with the given details.
        /// </summary>
        /// <param name="model">The new user information.</param>
        /// <returns>A File Not Found if the specified user doesn't exist.  The Update view if the
        /// update has failed validation, or the Index view if the update succeeds.</returns>
        /// @mapping POST /Admin/Update
        /// @notanon
        /// @role{GeneralAdmin}
        /// @antiforgery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == model.Id);
                if (user == null)
                    return HttpNotFound();

                user.Email = model.EmailAddress;

                var role = user.Roles.FirstOrDefault();
                if (role != null)
                    user.Roles.Remove(role);

                role = new IdentityUserRole();
                role.UserId = model.Id;
                role.RoleId = model.RoleId;
                user.Roles.Add(role);

                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// Adds the provided IdentityResult errors to the ModelState at the global error.
        /// </summary>
        /// <param name="result">The IdentityResult that needs reflecting in the ModelState.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}