using BikeTracker.Models.AccountViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// Controller which handles account management.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        [InjectionConstructor]
        public AccountController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager to use.</param>
        /// <param name="signInManager">The sign in manager to use.</param>
        /// <param name="urlHelper">The URL helper to use.</param>
        /// <remarks>
        /// This overload is used to allow dependency injection for testing.
        /// </remarks>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, UrlHelper urlHelper, IAuthenticationManager authManager = null)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Url = urlHelper ?? Url;
            _authManager = authManager;
        }

        /// <summary>
        /// Gets the sign in manager.
        /// </summary>
        /// <value>
        /// The sign in manager.
        /// </value>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Gets the authentication manager.
        /// </summary>
        /// <value>
        /// The authentication manager.
        /// </value>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authManager ?? HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Uses the provided callback code to confirm a user's email address.
        /// </summary>
        /// <param name="userId">The ID of the user the code belongs to.</param>
        /// <param name="code">The confirmation code.</param>
        /// <returns>
        /// The ConfirmEmail view if successful, otherwise the Error view.  In either case, no model.
        /// </returns>
        /// @mapping GET /Account/ConfirmEmail
        /// @anon
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            try
            {
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            catch (InvalidOperationException)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Starts the forgot password process.
        /// </summary>
        /// <returns>The ForgotPassword review</returns>
        /// @mapping GET /Account/ForgotPassword
        /// @anon
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Continues the forgotten password process.
        /// </summary>
        /// <param name="model">The model containing the user's identifying details.</param>
        /// <returns>
        /// If the model is completed properly, the ForgotPasswordConfirmation view (even if the user doesn't exist)
        ///
        /// If the model isn't completed properly, returns to the ForgotPassword view.
        /// </returns>
        /// @mapping POST /Account/ForgotPassword
        /// @anon
        /// @antiforgery
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user != null && (await UserManager.IsEmailConfirmedAsync(user.Id)))
                    await UserManager.GeneratePasswordResetEmailAsync(Url, user.Id);

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        /// <summary>
        /// Informs the user that their password has been reset.
        /// </summary>
        /// <returns>The ForgotPasswordConfirmation view.</returns>
        /// @mapping GET /Account/ForgotPasswordConfirmation
        /// @anon
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Starts the login process with the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The URL to return to after logging in.</param>
        /// <returns>The Login view with no model.</returns>
        /// @mapping GET /Account/Login
        /// @anon
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Attempts to log in with the provided details.
        /// </summary>
        /// <param name="model">The model containing the login details.</param>
        /// <param name="returnUrl">The URL to return to after logging in.</param>
        /// <returns>
        /// Returns a redirect to the result of passing returnUrl through <see cref="RedirectToLocal(string)"/>.
        /// </returns>
        /// <remarks>
        /// Currently does not lock out the user if the password is used too many times.
        /// </remarks>
        /// @mapping POST /Account/Login
        /// @anon
        /// @antiforgery
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindAsync(model.Email, model.Password);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Email address has not been confirmed.");
                    await UserManager.GenerateEmailConfirmationEmailAsync(Url, user.Id);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.Failure:
                default:
                    // This should only be reached if something breaks internally in the SignInManager
                    // as the code above already checks the username and password is valid.
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        /// <summary>
        /// Logs the user off.
        /// </summary>
        /// <returns>A redirect to Index/Home</returns>
        /// @mapping POST /Account/LogOff
        /// @notanon
        /// @antiforgery
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Accepts the password reset confirmation code and asks the user for a new password.
        /// </summary>
        /// <param name="code">The code that allows the password to be reset.</param>
        /// <returns>The ResetPassword view if the code is provided.  Otherwise returns an error.</returns>
        /// @mapping GET /Account/ResetPassword
        /// @anon
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return string.IsNullOrWhiteSpace(code) ? View("Error") : View();
        }

        /// <summary>
        /// Accepts the password reset confirmation code and new password and attempts to reset the password.
        /// </summary>
        /// <param name="model">The model containing the confirmation code and the new password.</param>
        /// <returns>
        /// A redirect to ResetPasswordConfirmation if password is reset or if the user doesn't exist.
        ///
        /// Otherwise redirects to ResetPassword view with model errors.
        /// </returns>
        /// @mapping POST /Account/ResetPassword
        /// @anon
        /// @antiforgery
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        /// <summary>
        /// Confirms to the user that their password has been reset.
        /// </summary>
        /// <returns>The ResetPasswordConfirmation view.</returns>
        /// @mapping GET /Account/ResetPasswordConfirmation
        /// @anon
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Adds the errors from the identity result to the ModelState.
        /// </summary>
        /// <param name="result">The result to process.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// Processes a redirect URI to ensure that it points to a local resource.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>
        /// If <paramref name="returnUrl"/> is a local resource, then returns a redirect to <paramref name="returnUrl"/>.
        /// Otherwise returns a redirection to Index/Home.
        /// </returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}