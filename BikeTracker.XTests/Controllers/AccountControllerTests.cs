using BikeTracker.Controllers;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace BikeTracker.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AccountControllerTests
    {
        private enum Result
        {
            VerificationFailure,
            ValidationFailure,
            RedirectToUri,
            RedirectToHome
        }

        [Fact]
        public void ConfirmationForResetPassword()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.ResetPasswordConfirmation();

            var view = result as ViewResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task ConfirmEmailBadToken()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, MockHelpers.BadToken, type: ResultType.BadRequest);
        }

        [Fact]
        public async Task ConfirmEmailBadUser()
        {
            await ConfirmEmail(MockHelpers.BadId, MockHelpers.GoodToken, type: ResultType.BadRequest);
        }

        [Fact]
        public async Task ConfirmEmailEmptyBoth()
        {
            await ConfirmEmail(string.Empty, string.Empty, false, ResultType.BadRequest);
        }

        [Fact]
        public async Task ConfirmEmailEmptyToken()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, string.Empty, false, ResultType.BadRequest);
        }

        [Fact]
        public async Task ConfirmEmailEmptyUser()
        {
            await ConfirmEmail(string.Empty, MockHelpers.GoodToken, false, ResultType.BadRequest);
        }

        [Fact]
        public async Task ConfirmEmailGoodData()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, MockHelpers.GoodToken);
        }

        [Fact]
        public async Task ConfirmEmailNoBoth()
        {
            await ConfirmEmail(null, null, false, ResultType.BadRequest);
        }

        [Fact]
        public async Task ConfirmEmailNoToken()
        {
            await ConfirmEmail(MockHelpers.UnconfirmedGoodId, null, false, ResultType.BadRequest);
        }

        [Fact]
        public async Task ConfirmEmailNoUser()
        {
            await ConfirmEmail(null, MockHelpers.GoodToken, false, ResultType.BadRequest);
        }

        [Fact]
        public void ConfirmForgotPassword()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.ForgotPasswordConfirmation();

            var view = result as ViewResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task ContinueForgotPasswordBadEmail()
        {
            // Note: this should look like a successful reset, but not actually send an email.
            await ContinueForgotPassword(MockHelpers.BadUsername, sendEmail: false);
        }

        [Fact]
        public async Task ContinueForgotPasswordGoodEmail()
        {
            await ContinueForgotPassword(MockHelpers.ConfirmedGoodUsername, MockHelpers.ConfirmedGoodId);
        }

        [Fact]
        public async Task ContinueForgotPasswordNoEmail()
        {
            await ContinueForgotPassword(null, sendEmail: false, expectedResult: ResultType.ModelError);
        }

        [Fact]
        public async Task ContinueForgotPasswordUncomfirmedEmail()
        {
            // Note: this should look like a successful reset, but not actually send an email.
            await ContinueForgotPassword(MockHelpers.UnconfirmedGoodUsername, sendEmail: false);
        }

        [Fact]
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
            Assert.NotNull(redirect);
            Assert.Equal("Index", redirect.RouteValues["action"]);
            Assert.Equal("Home", redirect.RouteValues["controller"]);
        }

        [Fact]
        public void MidResetPasswordEmptyCode()
        {
            MidResetPassword(string.Empty, ResultType.ModelError);
        }

        [Fact]
        public void MidResetPasswordGoodCode()
        {
            MidResetPassword(MockHelpers.GoodToken);
        }

        [Fact]
        public void MidResetPasswordNoCode()
        {
            MidResetPassword(null, ResultType.ModelError);
        }

        [Fact]
        public async Task ResetPasswordBadCode()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.BadToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername, expectedResult: ResultType.ModelError);
        }

        [Fact]
        public async Task ResetPasswordBadPassword()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, expectedResult: ResultType.ModelError);
        }

        [Fact]
        public async Task ResetPasswordBadUser()
        {
            // Should look like a success but not actually attempt to reset the password.
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.BadUsername, false);
        }

        [Fact]
        public async Task ResetPasswordGoodData()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername);
        }

        [Fact]
        public async Task ResetPasswordMismatchPassword()
        {
            await ResetPassword(MockHelpers.ConfirmedGoodId, MockHelpers.GoodToken, MockHelpers.ConfirmedGoodPassword, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, false, ResultType.ModelError);
        }

        [Fact]
        public void StartForgotPassword()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.ForgotPassword();

            var view = result as ViewResult;

            Assert.NotNull(view);
        }

        [Fact]
        public void StartLoginEmptyReturnUrl()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.Login(null) as ViewResult;
            Assert.NotNull(result);
            Assert.Null(result.ViewBag.ReturnUrl);
        }

        [Fact]
        public void StartLoginLocalReturnUrl()
        {
            var controller = MockHelpers.CreateAccountController();
            var result = controller.Login(MockHelpers.LocalUri) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(MockHelpers.LocalUri, result.ViewBag.ReturnUrl);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

            Assert.NotNull(view);

            switch (type)
            {
                case ResultType.BadRequest:
                    Assert.Equal("Error", view.ViewName);
                    break;

                case ResultType.Success:
                    Assert.NotEqual("Error", view.ViewName);
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
                    Assert.NotNull(redirect);
                    Assert.Equal("Account", redirect.RouteValues["controller"]);
                    Assert.Equal("ForgotPasswordConfirmation", redirect.RouteValues["action"]);
                    break;

                case ResultType.ModelError:
                    var view = result as ViewResult;
                    Assert.NotNull(view);
                    break;
            }
        }

        private Mock<ILogService> CreateMockLogService()
        {
            var result = new Mock<ILogService>(MockBehavior.Strict);

            result.Setup(l => l.LogUserLoggedIn(MockHelpers.ConfirmedGoodUsername)).Returns(Task.FromResult<object>(null));
            result.Setup(l => l.LogUserUpdated(MockHelpers.ConfirmedGoodUsername, MockHelpers.ConfirmedGoodUsername, It.Is<IEnumerable<string>>(s => s.Single() == "Password"))).Returns(Task.FromResult<object>(null));

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
                    Assert.NotNull(view);
                    Assert.Equal("Error", view.ViewName);
                    break;

                case ResultType.Success:
                    Assert.NotNull(view);
                    Assert.NotEqual("Error", view.ViewName);
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
            var logService = CreateMockLogService();

            var container = new UnityContainer();

            container.RegisterInstance(logService.Object);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

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
                    Assert.NotNull(view);
                    break;

                case ResultType.Success:
                    var redirect = result as RedirectToRouteResult;
                    Assert.NotNull(redirect);
                    Assert.Equal("ResetPasswordConfirmation", redirect.RouteValues["action"]);
                    Assert.Equal("Account", redirect.RouteValues["controller"]);
                    break;
            }

            if (attemptReset && expectedResult == ResultType.Success)
                logService.Verify(s => s.LogUserUpdated(email, email, It.Is<IEnumerable<string>>(i => i.Single() == "Password")));
            else
                logService.Verify(s => s.LogUserUpdated(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
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

                    Assert.NotNull(vr);
                    Assert.Equal(loginModel, vr.Model);
                    break;

                case Result.ValidationFailure:
                    vr = result as ViewResult;

                    userManager.Verify(m => m.FindAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                    signInManager.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
                    logService.Verify(l => l.LogUserLoggedIn(It.IsAny<string>()), Times.Never);

                    Assert.NotNull(vr);
                    Assert.Equal(loginModel, vr.Model);
                    break;

                case Result.RedirectToUri:
                    var rr = result as RedirectResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.SignInAsync(user, loginModel.RememberMe, false), Times.Once);
                    urlHelper.Verify(m => m.IsLocalUrl(returnUrl), Times.AtLeastOnce);
                    logService.Verify(l => l.LogUserLoggedIn(loginModel.Email));

                    Assert.NotNull(rr);
                    Assert.Equal(returnUrl, rr.Url);
                    Assert.Equal(false, rr.Permanent);
                    break;

                case Result.RedirectToHome:
                    var rtr = result as RedirectToRouteResult;

                    userManager.Verify(m => m.FindAsync(loginModel.Email, loginModel.Password), Times.AtLeastOnce);
                    signInManager.Verify(m => m.SignInAsync(user, loginModel.RememberMe, false), Times.Once);
                    urlHelper.Verify(m => m.IsLocalUrl(returnUrl), Times.AtLeastOnce);
                    logService.Verify(l => l.LogUserLoggedIn(loginModel.Email));

                    Assert.NotNull(rtr);
                    Assert.Equal("Index", rtr.RouteValues["action"]);
                    Assert.Equal("Home", rtr.RouteValues["controller"]);
                    Assert.Equal(false, rtr.Permanent);

                    break;

                default:
                    Assert.True(false, "Bad expected result.");
                    break;
            }
        }
    }
}