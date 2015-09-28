using BikeTracker.Models;
using BikeTracker.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    [Authorize(Roles = "GeneralAdmin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: Admin
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(string id, FormCollection collection)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                return HttpNotFound();

            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

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
    }
}