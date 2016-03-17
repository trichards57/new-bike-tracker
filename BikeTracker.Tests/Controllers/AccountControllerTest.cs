using BikeTracker.Controllers;
using BikeTracker.Models;
using BikeTracker.Tests.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {

        public AccountControllerTest()
        {
        }


        [TestMethod]
        public void StartLoginEmptyReturnUrl()
        {
            var controller = AccountMockHelpers.CreateAccountController();
            var result = controller.Login(null) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewBag.ReturnUrl);
        }

        [TestMethod]
        public void StartLoginLocalReturnUrl()
        {
            var controller = AccountMockHelpers.CreateAccountController();
            var result = controller.Login(AccountMockHelpers.LocalUri) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(AccountMockHelpers.LocalUri, result.ViewBag.ReturnUrl);
        }

        private enum Result
        {
            VerificationFailure,
            ValidationFailure,
            RedirectToUri,
            RedirectToHome
        }

        private async Task SubmitLogin(LoginViewModel loginModel, string returnUrl, Result expectedResult)
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var vals = Validate(loginModel);

            foreach (var v in vals)
            {
                foreach (var m in v.MemberNames)
                    controller.ModelState.AddModelError(m, v.ErrorMessage);
            }

            var result = await controller.Login(loginModel, returnUrl);

            ViewResult vr;

            switch (expectedResult)
            {
                case Result.VerificationFailure:
                    vr = result as ViewResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);

                    Assert.IsNotNull(vr);
                    Assert.AreEqual(loginModel, vr.Model);
                    break;
                case Result.ValidationFailure:
                    vr = result as ViewResult;

                    userManager.Verify(m => m.FindAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                    signInManager.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);

                    Assert.IsNotNull(vr);
                    Assert.AreEqual(loginModel, vr.Model);
                    break;
                case Result.RedirectToUri:
                    var rr = result as RedirectResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false), Times.Once);
                    urlHelper.Verify(m => m.IsLocalUrl(returnUrl), Times.AtLeastOnce);

                    Assert.IsNotNull(rr);
                    Assert.AreEqual(returnUrl, rr.Url);
                    Assert.AreEqual(false, rr.Permanent);
                    break;
                case Result.RedirectToHome:
                    var rtr = result as RedirectToRouteResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false), Times.Once);
                    urlHelper.Verify(m => m.IsLocalUrl(returnUrl), Times.AtLeastOnce);

                    Assert.IsNotNull(rtr);
                    Assert.AreEqual("Index", rtr.RouteValues["action"]);
                    Assert.AreEqual("Home", rtr.RouteValues["controller"]);
                    Assert.AreEqual(false, rtr.Permanent);

                    break;
                default:
                    Assert.Fail("Bad expected result.");
                    break;
            }
        }

        [TestMethod]
        public async Task SubmitLoginBadBoth()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.BadUsername,
                Password = AccountMockHelpers.BadPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginBadPassword()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.ConfirmedGoodUsername,
                Password = AccountMockHelpers.BadPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginBadUsername()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.BadUsername,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginNoDetails()
        {
            var loginModel = new LoginViewModel
            {
                Email = null,
                Password = null,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.ValidationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginNoPassword()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.ConfirmedGoodUsername,
                Password = null,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.ValidationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginNoUsername()
        {
            var loginModel = new LoginViewModel
            {
                Email = null,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.ValidationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelLocalUrlRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.ConfirmedGoodUsername,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, AccountMockHelpers.LocalUri, Result.RedirectToUri);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlConfirmedEmailNotRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.ConfirmedGoodUsername,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                RememberMe = false
            };

            await SubmitLogin(loginModel, null, Result.RedirectToHome);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlConfirmedEmailRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.ConfirmedGoodUsername,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.RedirectToHome);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlUnconfirmedEmailRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.UnconfirmedGoodUsername,
                Password = AccountMockHelpers.UnconfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelRemoteUrlRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = AccountMockHelpers.ConfirmedGoodUsername,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, AccountMockHelpers.ExternalUri, Result.RedirectToHome);
        }

        [TestMethod]
        public async Task ConfirmEmailGoodToken()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var res = await controller.ConfirmEmail(AccountMockHelpers.ConfirmedGoodId, AccountMockHelpers.GoodToken);

            userManager.Verify(a => a.ConfirmEmailAsync(AccountMockHelpers.ConfirmedGoodId, AccountMockHelpers.GoodToken));
            var view = res as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreNotEqual("Error", view.ViewName);
        }

        [TestMethod]
        public async Task ConfirmEmailNonExistentUser()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var res = await controller.ConfirmEmail(AccountMockHelpers.BadId, AccountMockHelpers.GoodToken);

            var view = res as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public async Task ConfirmEmailBadToken()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var res = await controller.ConfirmEmail(AccountMockHelpers.UnconfirmedGoodId, AccountMockHelpers.BadToken);

            userManager.Verify(a => a.ConfirmEmailAsync(AccountMockHelpers.UnconfirmedGoodId, AccountMockHelpers.BadToken));
            var view = res as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public async Task ConfirmEmailNoUser()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var res = await controller.ConfirmEmail(null, AccountMockHelpers.GoodToken);

            var view = res as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public async Task ConfirmEmailEmptyUser()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var res = await controller.ConfirmEmail(string.Empty, AccountMockHelpers.GoodToken);

            var view = res as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public async Task ConfirmEmailNoToken()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var res = await controller.ConfirmEmail(AccountMockHelpers.ConfirmedGoodId, null);

            var view = res as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public async Task ConfirmEmailEmptyToken()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var res = await controller.ConfirmEmail(AccountMockHelpers.ConfirmedGoodId, string.Empty);

            var view = res as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public void StartForgotPassword()
        {
            var controller = AccountMockHelpers.CreateAccountController();
            var result = controller.ForgotPassword();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void ConfirmForgotPassword()
        {
            var controller = AccountMockHelpers.CreateAccountController();
            var result = controller.ForgotPasswordConfirmation();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void ResetPasswordConfirmation()
        {
            var controller = AccountMockHelpers.CreateAccountController();
            var result = controller.ResetPasswordConfirmation();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void LogOff()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();
            var authManager = AccountMockHelpers.CreateMockAuthenticationManager();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object, authManager.Object);

            var result = controller.LogOff();

            var redirect = result as RedirectToRouteResult;

            authManager.Verify(a => a.SignOut(DefaultAuthenticationTypes.ApplicationCookie));
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Index", redirect.RouteValues["action"]);
            Assert.AreEqual("Home", redirect.RouteValues["controller"]);
        }

        [TestMethod]
        public void MidResetPasswordGoodCode()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var result = controller.ResetPassword(AccountMockHelpers.GoodToken);

            var view = result as ViewResult;
            
            Assert.IsNotNull(view);
            Assert.AreNotEqual("Error", view.ViewName);
        }

        [TestMethod]
        public void MidResetPasswordEmptyCode()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var result = controller.ResetPassword(string.Empty);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public void MidResetPasswordNoCode()
        {
            var controller = AccountMockHelpers.CreateAccountController();

            var result = controller.ResetPassword((string)null);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Error", view.ViewName);
        }

        [TestMethod]
        public async Task ResetPasswordGoodData()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ResetPasswordViewModel
            {
                Code = AccountMockHelpers.GoodToken,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = AccountMockHelpers.ConfirmedGoodPassword,
                Email = AccountMockHelpers.ConfirmedGoodUsername
            };

            var result = await controller.ResetPassword(model);
            var redirect = result as RedirectToRouteResult;

            userManager.Verify(a => a.ResetPasswordAsync(AccountMockHelpers.ConfirmedGoodId, AccountMockHelpers.GoodToken, AccountMockHelpers.ConfirmedGoodPassword));
            Assert.IsNotNull(redirect);
            Assert.AreEqual("ResetPasswordConfirmation", redirect.RouteValues["action"]);
            Assert.AreEqual("Account", redirect.RouteValues["controller"]);
        }

        [TestMethod]
        public async Task ResetPasswordBadPassword()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ResetPasswordViewModel
            {
                Code = AccountMockHelpers.GoodToken,
                Password = AccountMockHelpers.BadPassword,
                ConfirmPassword = AccountMockHelpers.BadPassword,
                Email = AccountMockHelpers.ConfirmedGoodUsername
            };

            var result = await controller.ResetPassword(model);
            var view = result as ViewResult;

            userManager.Verify(a => a.ResetPasswordAsync(AccountMockHelpers.ConfirmedGoodId, AccountMockHelpers.GoodToken, AccountMockHelpers.BadPassword));
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ResetPasswordBadCode()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ResetPasswordViewModel
            {
                Code = AccountMockHelpers.BadToken,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = AccountMockHelpers.ConfirmedGoodPassword,
                Email = AccountMockHelpers.ConfirmedGoodUsername
            };

            var result = await controller.ResetPassword(model);
            var view = result as ViewResult;

            userManager.Verify(a => a.ResetPasswordAsync(AccountMockHelpers.ConfirmedGoodId, AccountMockHelpers.BadToken, AccountMockHelpers.ConfirmedGoodPassword));
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ResetPasswordMismatchPassword()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ResetPasswordViewModel
            {
                Code = AccountMockHelpers.GoodToken,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = AccountMockHelpers.BadPassword,
                Email = AccountMockHelpers.ConfirmedGoodUsername
            };

            var vals = Validate(model);

            foreach (var v in vals)
            {
                if (!v.MemberNames.Any())
                { 
                    controller.ModelState.AddModelError("model", v.ErrorMessage);
                }
                else
                {
                    foreach (var m in v.MemberNames)
                        controller.ModelState.AddModelError(m, v.ErrorMessage);
                }
            }

            var result = await controller.ResetPassword(model);
            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ResetPasswordBadUser()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ResetPasswordViewModel
            {
                Code = AccountMockHelpers.GoodToken,
                Password = AccountMockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = AccountMockHelpers.ConfirmedGoodPassword,
                Email = AccountMockHelpers.BadUsername
            };

            var result = await controller.ResetPassword(model);
            var redirect = result as RedirectToRouteResult;

            Assert.IsNotNull(redirect);
            Assert.AreEqual("ResetPasswordConfirmation", redirect.RouteValues["action"]);
            Assert.AreEqual("Account", redirect.RouteValues["controller"]);
        }

        [TestMethod]
        public async Task ContinueForgotPasswordGoodEmail()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ForgotPasswordViewModel
            {
                Email = AccountMockHelpers.ConfirmedGoodUsername
            };

            var vals = Validate(model);

            foreach (var v in vals)
            {
                foreach (var m in v.MemberNames)
                    controller.ModelState.AddModelError(m, v.ErrorMessage);
            }

            var result = await controller.ForgotPassword(model);
            var redirect = result as RedirectToRouteResult;

            userManager.Verify(a => a.GeneratePasswordResetEmailAsync(urlHelper.Object, AccountMockHelpers.ConfirmedGoodId));
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Account", redirect.RouteValues["controller"]);
            Assert.AreEqual("ForgotPasswordConfirmation", redirect.RouteValues["action"]);
        }

        [TestMethod]
        public async Task ContinueForgotPasswordBadEmail()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ForgotPasswordViewModel
            {
                Email = AccountMockHelpers.BadUsername
            };

            var vals = Validate(model);

            foreach (var v in vals)
            {
                foreach (var m in v.MemberNames)
                    controller.ModelState.AddModelError(m, v.ErrorMessage);
            }

            var result = await controller.ForgotPassword(model);
            var redirect = result as RedirectToRouteResult;

            Assert.IsNotNull(redirect);
            Assert.AreEqual("Account", redirect.RouteValues["controller"]);
            Assert.AreEqual("ForgotPasswordConfirmation", redirect.RouteValues["action"]);

            // Note: this should look like a successful reset, but not actually send an email.
        }

        [TestMethod]
        public async Task ContinueForgotPasswordUncomfirmedEmail()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ForgotPasswordViewModel
            {
                Email = AccountMockHelpers.UnconfirmedGoodUsername
            };

            var vals = Validate(model);

            foreach (var v in vals)
            {
                foreach (var m in v.MemberNames)
                    controller.ModelState.AddModelError(m, v.ErrorMessage);
            }

            var result = await controller.ForgotPassword(model);
            var redirect = result as RedirectToRouteResult;

            Assert.IsNotNull(redirect);
            Assert.AreEqual("Account", redirect.RouteValues["controller"]);
            Assert.AreEqual("ForgotPasswordConfirmation", redirect.RouteValues["action"]);

            // Note: this should look like a successful reset, but not actually send an email.
        }

        [TestMethod]
        public async Task ContinueForgotPasswordNoEmail()
        {
            var userManager = AccountMockHelpers.CreateMockUserManager();
            var signInManager = AccountMockHelpers.CreateMockSignInManager();
            var urlHelper = AccountMockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ForgotPasswordViewModel
            {
                Email = null,
            };

            var vals = Validate(model);

            foreach (var v in vals)
            {
                foreach (var m in v.MemberNames)
                    controller.ModelState.AddModelError(m, v.ErrorMessage);
            }

            var result = await controller.ForgotPassword(model);
            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        private List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);

            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);

            return results;
        }
    }
}