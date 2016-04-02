using BikeTracker.Controllers;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AccountControllerTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly string TestUsername;

        public AccountControllerTests()
        {
            TestUsername = Fixture.Create<string>();
        }

        private enum Result
        {
            VerificationFailure,
            ValidationFailure,
            RedirectToUri,
            RedirectToHome
        }

        [TestMethod]
        public void ConfirmationForResetPassword()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.ResetPasswordConfirmation();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ConfirmEmailBadToken()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, MockHelpers.BadToken, type: ResultType.BadRequest);
        }

        [TestMethod]
        public async Task ConfirmEmailBadUser()
        {
            await ConfirmEmail(MockHelpers.BadId, MockHelpers.GoodToken, type: ResultType.BadRequest);
        }

        [TestMethod]
        public async Task ConfirmEmailEmptyBoth()
        {
            await ConfirmEmail(string.Empty, string.Empty, false, ResultType.BadRequest);
        }

        [TestMethod]
        public async Task ConfirmEmailEmptyToken()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, string.Empty, false, ResultType.BadRequest);
        }

        [TestMethod]
        public async Task ConfirmEmailEmptyUser()
        {
            await ConfirmEmail(string.Empty, MockHelpers.GoodToken, false, ResultType.BadRequest);
        }

        [TestMethod]
        public async Task ConfirmEmailGoodData()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, MockHelpers.GoodToken);
        }

        [TestMethod]
        public async Task ConfirmEmailNoBoth()
        {
            await ConfirmEmail(null, null, false, ResultType.BadRequest);
        }

        [TestMethod]
        public async Task ConfirmEmailNoToken()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, null, false, ResultType.BadRequest);
        }

        [TestMethod]
        public async Task ConfirmEmailNoUser()
        {
            await ConfirmEmail(null, MockHelpers.GoodToken, false, ResultType.BadRequest);
        }

        [TestMethod]
        public void ConfirmForgotPassword()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.ForgotPasswordConfirmation();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ContinueForgotPasswordBadEmail()
        {
            // Note: this should look like a successful reset, but not actually send an email.
            await ContinueForgotPassword(MockHelpers.BadUsername, sendEmail: false);
        }

        [TestMethod]
        public async Task ContinueForgotPasswordGoodEmail()
        {
            await ContinueForgotPassword(MockHelpers.ConfirmedGoodUsername, MockHelpers.ConfirmedGoodId);
        }

        [TestMethod]
        public async Task ContinueForgotPasswordNoEmail()
        {
            await ContinueForgotPassword(null, sendEmail: false, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task ContinueForgotPasswordUncomfirmedEmail()
        {
            // Note: this should look like a successful reset, but not actually send an email.
            await ContinueForgotPassword(MockHelpers.UnconfirmedGoodUsername, sendEmail: false);
        }

        [TestMethod]
        public void LogOff()
        {
            var userManager = MockHelpers.CreateMockUserManager();
            var signInManager = MockHelpers.CreateMockSignInManager();
            var urlHelper = MockHelpers.CreateMockUrlHelper();
            var authManager = MockHelpers.CreateMockAuthenticationManager();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object, authManager.Object);

            var result = controller.LogOff();

            var redirect = result as RedirectToRouteResult;

            authManager.Verify(a => a.SignOut(DefaultAuthenticationTypes.ApplicationCookie));
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Index", redirect.RouteValues["action"]);
            Assert.AreEqual("Home", redirect.RouteValues["controller"]);
        }

        [TestMethod]
        public void MidResetPasswordEmptyCode()
        {
            MidResetPassword(string.Empty, ResultType.ModelError);
        }

        [TestMethod]
        public void MidResetPasswordGoodCode()
        {
            MidResetPassword(MockHelpers.GoodToken);
        }

        [TestMethod]
        public void MidResetPasswordNoCode()
        {
            MidResetPassword(null, ResultType.ModelError);
        }

        [TestMethod]
        public async Task ResetPasswordBadCode()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.BadToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task ResetPasswordBadPassword()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task ResetPasswordBadUser()
        {
            // Should look like a success but not actually attempt to reset the password.
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.BadUsername, false);
        }

        [TestMethod]
        public async Task ResetPasswordGoodData()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername);
        }

        [TestMethod]
        public async Task ResetPasswordMismatchPassword()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, false, ResultType.ModelError);
        }

        [TestMethod]
        public void StartForgotPassword()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.ForgotPassword();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void StartLoginEmptyReturnUrl()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.Login(null) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewBag.ReturnUrl);
        }

        [TestMethod]
        public void StartLoginLocalReturnUrl()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.Login(MockHelpers.LocalUri) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(MockHelpers.LocalUri, result.ViewBag.ReturnUrl);
        }

        [TestMethod]
        public async Task SubmitLoginBadBoth()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.BadUsername,
                Password = MockHelpers.BadPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginBadPassword()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.ConfirmedGoodUsername,
                Password = MockHelpers.BadPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginBadUsername()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.BadUsername,
                Password = MockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, null, Result.VerificationFailure);
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

            await SubmitLogin(loginModel, null, null, Result.ValidationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginNoPassword()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.ConfirmedGoodUsername,
                Password = null,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, null, Result.ValidationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginNoUsername()
        {
            var loginModel = new LoginViewModel
            {
                Email = null,
                Password = MockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, null, Result.ValidationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelLocalUrlRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.ConfirmedGoodUsername,
                Password = MockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, MockHelpers.ConfirmedGoodUser, MockHelpers.LocalUri, Result.RedirectToUri);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlConfirmedEmailNotRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.ConfirmedGoodUsername,
                Password = MockHelpers.ConfirmedGoodPassword,
                RememberMe = false
            };

            await SubmitLogin(loginModel, MockHelpers.ConfirmedGoodUser, null, Result.RedirectToHome);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlConfirmedEmailRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.ConfirmedGoodUsername,
                Password = MockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, MockHelpers.ConfirmedGoodUser, null, Result.RedirectToHome);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlUnconfirmedEmailRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.UnconfirmedGoodUsername,
                Password = MockHelpers.UnconfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelRemoteUrlRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = MockHelpers.ConfirmedGoodUsername,
                Password = MockHelpers.ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, MockHelpers.ConfirmedGoodUser, MockHelpers.ExternalUri, Result.RedirectToHome);
        }

        private async Task ConfirmEmail(string userId, string code, bool attemptConfirm = true, ResultType type = ResultType.Success)
        {
            var userManager = MockHelpers.CreateMockUserManager();
            var signInManager = MockHelpers.CreateMockSignInManager();

            var controller = new AccountController(userManager.Object, signInManager.Object, null);

            var res = await controller.ConfirmEmail(userId, code);
            var view = res as ViewResult;

            if (attemptConfirm)
                userManager.Verify(a => a.ConfirmEmailAsync(userId, code));
            else
                userManager.Verify(a => a.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            Assert.IsNotNull(view);

            switch (type)
            {
                case ResultType.BadRequest:
                    Assert.AreEqual("Error", view.ViewName);
                    break;

                case ResultType.Success:
                    Assert.AreNotEqual("Error", view.ViewName);
                    break;
            }
        }

        private async Task ContinueForgotPassword(string email, string id = null, bool sendEmail = true, ResultType expectedResult = ResultType.Success)
        {
            var userManager = MockHelpers.CreateMockUserManager();
            var signInManager = MockHelpers.CreateMockSignInManager();
            var urlHelper = MockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ForgotPasswordViewModel
            {
                Email = email
            };

            MockHelpers.Validate(model, controller);

            var result = await controller.ForgotPassword(model);

            if (sendEmail)
                userManager.Verify(a => a.GeneratePasswordResetEmailAsync(urlHelper.Object, id));
            else
                userManager.Verify(a => a.GeneratePasswordResetEmailAsync(urlHelper.Object, It.IsAny<string>()), Times.Never);

            switch (expectedResult)
            {
                case ResultType.Success:
                    var redirect = result as RedirectToRouteResult;
                    Assert.IsNotNull(redirect);
                    Assert.AreEqual("Account", redirect.RouteValues["controller"]);
                    Assert.AreEqual("ForgotPasswordConfirmation", redirect.RouteValues["action"]);
                    break;

                case ResultType.ModelError:
                    var view = result as ViewResult;
                    Assert.IsNotNull(view);
                    break;
            }
        }

        private Mock<ILogService> CreateMockLogService()
        {
            var result = new Mock<ILogService>(MockBehavior.Strict);

            result.Setup(l => l.LogUserLoggedIn(MockHelpers.ConfirmedGoodUsername)).Returns(Task.FromResult<object>(null));

            return result;
        }

        private void MidResetPassword(string code, ResultType expectedResult = ResultType.Success)
        {
            var controller = MockHelpers.CreateAccountController();

            var result = controller.ResetPassword(code);

            var view = result as ViewResult;

            switch (expectedResult)
            {
                case ResultType.ModelError:
                    Assert.IsNotNull(view);
                    Assert.AreEqual("Error", view.ViewName);
                    break;

                case ResultType.Success:
                    Assert.IsNotNull(view);
                    Assert.AreNotEqual("Error", view.ViewName);
                    break;
            }
        }

        private async Task ResetPassword(string userId, string token, string password, string email, bool attemptReset = true, ResultType expectedResult = ResultType.Success)
        {
            await ResetPassword(userId, token, password, password, email, attemptReset, expectedResult);
        }

        private async Task ResetPassword(string userId, string token, string password, string passwordConfirm, string email, bool attemptReset = true, ResultType expectedResult = ResultType.Success)
        {
            var userManager = MockHelpers.CreateMockUserManager();
            var signInManager = MockHelpers.CreateMockSignInManager();
            var urlHelper = MockHelpers.CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);

            var model = new ResetPasswordViewModel
            {
                Code = token,
                Password = password,
                ConfirmPassword = passwordConfirm,
                Email = email
            };

            MockHelpers.Validate(model, controller);

            var result = await controller.ResetPassword(model);

            if (attemptReset)
                userManager.Verify(a => a.ResetPasswordAsync(userId, token, password));

            switch (expectedResult)
            {
                case ResultType.ModelError:
                    var view = result as ViewResult;
                    Assert.IsNotNull(view);
                    break;

                case ResultType.Success:
                    var redirect = result as RedirectToRouteResult;
                    Assert.IsNotNull(redirect);
                    Assert.AreEqual("ResetPasswordConfirmation", redirect.RouteValues["action"]);
                    Assert.AreEqual("Account", redirect.RouteValues["controller"]);
                    break;
            }
        }

        private async Task SubmitLogin(LoginViewModel loginModel, ApplicationUser user, string returnUrl, Result expectedResult)
        {
            var userManager = MockHelpers.CreateMockUserManager();
            var signInManager = MockHelpers.CreateMockSignInManager();
            var urlHelper = MockHelpers.CreateMockUrlHelper();
            var logService = CreateMockLogService();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object, logService: logService.Object);

            MockHelpers.Validate(loginModel, controller);

            var result = await controller.Login(loginModel, returnUrl);

            ViewResult vr;

            switch (expectedResult)
            {
                case Result.VerificationFailure:
                    vr = result as ViewResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
                    logService.Verify(l => l.LogUserLoggedIn(It.IsAny<string>()), Times.Never);

                    Assert.IsNotNull(vr);
                    Assert.AreEqual(loginModel, vr.Model);
                    break;

                case Result.ValidationFailure:
                    vr = result as ViewResult;

                    userManager.Verify(m => m.FindAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                    signInManager.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
                    logService.Verify(l => l.LogUserLoggedIn(It.IsAny<string>()), Times.Never);

                    Assert.IsNotNull(vr);
                    Assert.AreEqual(loginModel, vr.Model);
                    break;

                case Result.RedirectToUri:
                    var rr = result as RedirectResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.SignInAsync(user, loginModel.RememberMe, false), Times.Once);
                    urlHelper.Verify(m => m.IsLocalUrl(returnUrl), Times.AtLeastOnce);
                    logService.Verify(l => l.LogUserLoggedIn(loginModel.Email));

                    Assert.IsNotNull(rr);
                    Assert.AreEqual(returnUrl, rr.Url);
                    Assert.AreEqual(false, rr.Permanent);
                    break;

                case Result.RedirectToHome:
                    var rtr = result as RedirectToRouteResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.SignInAsync(user, loginModel.RememberMe, false), Times.Once);
                    urlHelper.Verify(m => m.IsLocalUrl(returnUrl), Times.AtLeastOnce);
                    logService.Verify(l => l.LogUserLoggedIn(loginModel.Email));

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
    }
}