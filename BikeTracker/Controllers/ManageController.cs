using BikeTracker.Controllers.Filters;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    public class ManageController : UserBaseController
    {
        private ILogService LogService
        {
            get
            {
                if (logService == null)
                    logService = DependencyResolver.Current.GetService<ILogService>();

                return logService;
            }
        }

        private ILogService logService;

        [InjectionConstructor, ExcludeFromCodeCoverage]
        public ManageController()
        {
        }

        public ManageController(IUserManager userManager, ISignInManager signInManager, ILogService logService)
            : base(userManager, signInManager)
        {
            this.logService = logService;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        //
        // GET: /Manage/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                await LogService.LogUserUpdated(user.UserName, user.UserName, new[] { "Password" });
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/Index
        [AuthorizePasswordExpires]
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var userId = User.Identity.GetUserId();
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}