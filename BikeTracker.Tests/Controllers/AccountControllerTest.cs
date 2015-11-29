using BikeTracker.Controllers;
using BikeTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private const string BadPassword = "Pass4321";
        private const string BadUsername = "tony.richards@bath.edu";
        private const string ConfirmedGoodId = "49DA7831-E0B6-4948-914D-D2C52BC81C0F";
        private const string ConfirmedGoodPassword = "Pass1234";
        private const string ConfirmedGoodUsername = "trichards57@gmail.com";
        private const string ExternalUri = "http://www.google.com";
        private const string LocalUri = "http://localhost:1234";
        private const string UnconfirmedGoodId = "3021EF55-0171-41BD-8665-6B0A56A70A1F";
        private const string UnconfirmedGoodPassword = "Pass.5678";
        private const string UnconfirmedGoodUsername = "trichards58@gmail.com";

        private readonly ApplicationUser ConfirmedGoodUser = new ApplicationUser
        {
            Id = ConfirmedGoodId,
            Email = ConfirmedGoodUsername,
            UserName = ConfirmedGoodUsername,
            EmailConfirmed = true
        };

        private readonly ApplicationUser UnconfirmedGoodUser = new ApplicationUser
        {
            Id = UnconfirmedGoodId,
            Email = UnconfirmedGoodUsername,
            UserName = UnconfirmedGoodUsername,
            EmailConfirmed = false
        };

        [TestMethod]
        public void StartLoginEmptyReturnUrl()
        {
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var urlHelper = CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);
            var result = controller.Login(null) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewBag.ReturnUrl);
        }

        [TestMethod]
        public void StartLoginLocalReturnUrl()
        {
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var urlHelper = CreateMockUrlHelper();

            var controller = new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);
            var result = controller.Login(LocalUri) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(LocalUri, result.ViewBag.ReturnUrl);
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
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var urlHelper = CreateMockUrlHelper();

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
                Email = BadUsername,
                Password = BadPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginBadPassword()
        {
            var loginModel = new LoginViewModel
            {
                Email = ConfirmedGoodUsername,
                Password = BadPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginBadUsername()
        {
            var loginModel = new LoginViewModel
            {
                Email = BadUsername,
                Password = ConfirmedGoodPassword,
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
                Email = ConfirmedGoodUsername,
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
                Password = ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.ValidationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelLocalUrlRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = ConfirmedGoodUsername,
                Password = ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, LocalUri, Result.RedirectToUri);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlConfirmedEmailNotRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = ConfirmedGoodUsername,
                Password = ConfirmedGoodPassword,
                RememberMe = false
            };

            await SubmitLogin(loginModel, null, Result.RedirectToUri);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlConfirmedEmailRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = ConfirmedGoodUsername,
                Password = ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.RedirectToHome);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelNoUrlUnconfirmedEmailRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = UnconfirmedGoodUsername,
                Password = UnconfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, null, Result.VerificationFailure);
        }

        [TestMethod]
        public async Task SubmitLoginValidModelRemoteUrlRemembered()
        {
            var loginModel = new LoginViewModel
            {
                Email = ConfirmedGoodUsername,
                Password = ConfirmedGoodPassword,
                RememberMe = true
            };

            await SubmitLogin(loginModel, ExternalUri, Result.RedirectToHome);
        }

        private Mock<ApplicationSignInManager> CreateMockSignInManager()
        {
            var userManager = CreateMockUserManager();
            var authManger = new Mock<IAuthenticationManager>(MockBehavior.Strict);
            var signInManager = new Mock<ApplicationSignInManager>(MockBehavior.Strict, userManager.Object, authManger.Object);

            signInManager.Setup(m =>
                m.PasswordSignInAsync(It.IsAny<string>(),
                                      It.IsAny<string>(),
                                      It.IsAny<bool>(),
                                      It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.Failure);
            signInManager.Setup(m =>
                m.PasswordSignInAsync(It.Is<string>(s => s == ConfirmedGoodUsername || s == UnconfirmedGoodUsername),
                                      It.Is<string>(s => s == ConfirmedGoodPassword || s == UnconfirmedGoodPassword),
                                      It.IsAny<bool>(),
                                      It.Is<bool>(b => b == false)))
                .ReturnsAsync(SignInStatus.Success);

            return signInManager;
        }

        private Mock<UrlHelper> CreateMockUrlHelper()
        {
            var urlHelper = new Mock<UrlHelper>(MockBehavior.Strict);

            urlHelper.Setup(m =>
                m.IsLocalUrl(It.IsAny<string>()))
                .Returns(false);

            urlHelper.Setup(m =>
                m.IsLocalUrl(It.Is<string>(s => s != null && s.StartsWith(LocalUri))))
                .Returns(true);

            return urlHelper;
        }

        private Mock<ApplicationUserManager> CreateMockUserManager()
        {
            var userStore = new Mock<IUserStore<ApplicationUser, string>>(MockBehavior.Strict);
            var userManager = new Mock<ApplicationUserManager>(MockBehavior.Strict, userStore.Object);
            userManager.Setup(m => m.FindAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(null);
            userManager.Setup(m =>
                m.FindAsync(It.Is<string>(s => s == ConfirmedGoodUsername),
                            It.Is<string>(s => s == ConfirmedGoodPassword)))
                .ReturnsAsync(ConfirmedGoodUser);
            userManager.Setup(m =>
                m.FindAsync(It.Is<string>(s => s == UnconfirmedGoodUsername),
                            It.Is<string>(s => s == UnconfirmedGoodPassword)))
                .ReturnsAsync(UnconfirmedGoodUser);
            userManager.Setup(m =>
                m.GenerateEmailConfirmationEmailAsync(It.IsAny<UrlHelper>(),
                                                      It.Is<string>(s => s == ConfirmedGoodId ||
                                                                         s == UnconfirmedGoodId)))
                .Returns(Task.FromResult<object>(null));

            return userManager;
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