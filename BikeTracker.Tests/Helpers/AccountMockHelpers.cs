using BikeTracker.Controllers;
using BikeTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Tests.Helpers
{
    public class AccountMockHelpers
    {
        private static readonly Fixture fixture = new Fixture();
        public static readonly string BadPassword;
        public static readonly string BadUsername;
        public static readonly string BadId;
        public static readonly string ConfirmedGoodId;
        public static readonly string ConfirmedGoodPassword;
        public static readonly string ConfirmedGoodUsername;
        public static readonly string GoodToken;
        public static readonly string BadToken;
        public static readonly string ExternalUri;
        public static readonly string LocalUri;
        public static readonly string UnconfirmedGoodId;
        public static readonly string UnconfirmedGoodPassword;
        public static readonly string UnconfirmedGoodUsername;
        public static readonly ApplicationUser ConfirmedGoodUser;
        public static readonly ApplicationUser UnconfirmedGoodUser;

        static AccountMockHelpers()
        {
            BadPassword = fixture.Create<string>();
            BadUsername = fixture.Create<MailAddress>().Address;
            BadId = fixture.Create<string>();
            ConfirmedGoodId = fixture.Create<Guid>().ToString("D");
            ConfirmedGoodPassword = fixture.Create<string>();
            ConfirmedGoodUsername = fixture.Create<MailAddress>().Address;
            GoodToken = fixture.Create<string>();
            BadToken = fixture.Create<string>();
            ExternalUri = fixture.Create<Uri>().ToString();
            LocalUri = fixture.Create<Uri>().ToString();
            UnconfirmedGoodId = fixture.Create<Guid>().ToString("D");
            UnconfirmedGoodPassword = fixture.Create<string>();
            UnconfirmedGoodUsername = fixture.Create<MailAddress>().Address;
            ConfirmedGoodUser = new ApplicationUser
            {
                Id = ConfirmedGoodId,
                Email = ConfirmedGoodUsername,
                UserName = ConfirmedGoodUsername,
                EmailConfirmed = true
            };
            UnconfirmedGoodUser = new ApplicationUser
            {
                Id = UnconfirmedGoodId,
                Email = UnconfirmedGoodUsername,
                UserName = UnconfirmedGoodUsername,
                EmailConfirmed = false
            };
        }

        public static AccountController CreateAccountController()
        {
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var urlHelper = CreateMockUrlHelper();

            return new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);
        }

        public static Mock<ApplicationSignInManager> CreateMockSignInManager()
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

        public static Mock<IAuthenticationManager> CreateMockAuthenticationManager()
        {
            var authManager = new Mock<IAuthenticationManager>(MockBehavior.Strict);

            authManager.Setup(a => a.SignOut(It.Is<string>(s => s == DefaultAuthenticationTypes.ApplicationCookie)));

            return authManager;
        }

        public static Mock<UrlHelper> CreateMockUrlHelper()
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

        public static Mock<ApplicationUserManager> CreateMockUserManager()
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

            userManager.Setup(m =>
                m.ConfirmEmailAsync(It.Is<string>(s => s == ConfirmedGoodId),
                                    It.Is<string>(s => s == GoodToken)))
                                    .Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(m =>
                m.ConfirmEmailAsync(It.Is<string>(s => s == UnconfirmedGoodId),
                                    It.Is<string>(s => s == BadToken)))
                                    .Returns(Task.FromResult(new IdentityResult("Bad Token")));

            userManager.Setup(m =>
                m.ConfirmEmailAsync(It.Is<string>(s => s == BadId),
                                    It.IsAny<string>()))
                                    .Throws(new InvalidOperationException());

            userManager.Setup(m =>
                m.FindByNameAsync(It.Is<string>(s => s == ConfirmedGoodUsername)))
                                .Returns(Task.FromResult(ConfirmedGoodUser));
            userManager.Setup(m =>
                m.FindByNameAsync(It.Is<string>(s => s == UnconfirmedGoodUsername)))
                                .Returns(Task.FromResult(UnconfirmedGoodUser));
            userManager.Setup(m =>
                m.FindByNameAsync(It.Is<string>(s => s == BadUsername)))
                                .Returns(Task.FromResult<ApplicationUser>(null));

            userManager.Setup(m =>
                m.IsEmailConfirmedAsync(It.Is<string>(s => s == ConfirmedGoodId)))
                                .Returns(Task.FromResult(true));
            userManager.Setup(m =>
                m.IsEmailConfirmedAsync(It.Is<string>(s => s == UnconfirmedGoodId)))
                                .Returns(Task.FromResult(false));

            userManager.Setup(m =>
                m.GeneratePasswordResetEmailAsync(It.IsAny<UrlHelper>(), It.Is<string>(s => s == ConfirmedGoodId)))
                                .Returns(Task.FromResult<object>(null));

            userManager.Setup(m =>
                m.ResetPasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == GoodToken), It.Is<string>(s => s == ConfirmedGoodPassword)))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManager.Setup(m =>
                m.ResetPasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == GoodToken), It.Is<string>(s => s == BadPassword)))
                .Returns(Task.FromResult(new IdentityResult("Password isn't good enough.")));
            userManager.Setup(m =>
                m.ResetPasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == BadToken), It.Is<string>(s => s == ConfirmedGoodPassword)))
                .Returns(Task.FromResult(new IdentityResult("Bad Token.")));
            return userManager;
        }
    }
}
