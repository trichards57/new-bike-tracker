using BikeTracker.Controllers;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Models.LocationModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BikeTracker.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class MockHelpers
    {
        public static readonly string BadId;
        public static readonly string BadPassword;
        public static readonly string BadToken;
        public static readonly string BadUsername;
        public static readonly ClaimsIdentity ConfirmedClaimsIdentity;
        public static readonly string ConfirmedGoodId;
        public static readonly string ConfirmedGoodPassword;
        public static readonly ApplicationUser ConfirmedGoodUser;
        public static readonly string ConfirmedGoodUsername;
        public static readonly string ExternalUri;
        public static readonly string GoodToken;
        public static readonly string LocalUri;
        public static readonly string UnconfirmedGoodId;
        public static readonly string UnconfirmedGoodPassword;
        public static readonly ApplicationUser UnconfirmedGoodUser;
        public static readonly string UnconfirmedGoodUsername;
        private static readonly Fixture fixture = new Fixture();

        static MockHelpers()
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

            ConfirmedClaimsIdentity = new ClaimsIdentity();
            ConfirmedClaimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, ConfirmedGoodId));
        }

        public static AccountController CreateAccountController()
        {
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var urlHelper = CreateMockUrlHelper();

            return new AccountController(userManager.Object, signInManager.Object, urlHelper.Object);
        }

        public static ManageController CreateManageController()
        {
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var httpContext = CreateMockHttpContext();

            var controller = new ManageController(userManager.Object, signInManager.Object);
            var context = new ControllerContext();
            context.HttpContext = httpContext.Object;
            controller.ControllerContext = context;

            return controller;
        }

        public static Mock<IAuthenticationManager> CreateMockAuthenticationManager()
        {
            var authManager = new Mock<IAuthenticationManager>(MockBehavior.Strict);

            authManager.Setup(a => a.SignOut(It.Is<string>(s => s == DefaultAuthenticationTypes.ApplicationCookie)));

            return authManager;
        }

        public static Mock<HttpContextBase> CreateMockHttpContext()
        {
            var context = new Mock<HttpContextBase>(MockBehavior.Strict);

            context.SetupGet(h => h.User.Identity).Returns(ConfirmedClaimsIdentity);

            return context;
        }

        public static Mock<ISignInManager> CreateMockSignInManager()
        {
            var userManager = CreateMockUserManager();
            var authManger = new Mock<IAuthenticationManager>(MockBehavior.Strict);
            var signInManager = new Mock<ISignInManager>(MockBehavior.Strict);

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

            signInManager.Setup(m =>
                m.SignInAsync(It.Is<ApplicationUser>(u => u == ConfirmedGoodUser), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult<object>(null));

            return signInManager;
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

        public static Mock<DbSet<LocationRecord>> CreateMockLocationDbSet(IList<LocationRecord> records)
        {
            var mockLocationEntrySet = new Mock<DbSet<LocationRecord>>();

            var data = records.AsQueryable();

            mockLocationEntrySet.Setup(e => e.Add(It.IsAny<LocationRecord>())).Callback<LocationRecord>(i => records.Add(i));

            mockLocationEntrySet.As<IDbAsyncEnumerable<LocationRecord>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<LocationRecord>(data.GetEnumerator()));

            mockLocationEntrySet.As<IQueryable<LocationRecord>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<LocationRecord>(data.Provider));

            mockLocationEntrySet.As<IQueryable<LocationRecord>>().Setup(m => m.Expression).Returns(data.Expression);
            mockLocationEntrySet.As<IQueryable<LocationRecord>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockLocationEntrySet.As<IQueryable<LocationRecord>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockLocationEntrySet;
        }


        public static Mock<IPrincipal> CreateMockPrincipal(string username)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, username));

            var mockPrinciple = new Mock<IPrincipal>();
            mockPrinciple.SetupGet(i => i.Identity).Returns(identity);

            return mockPrinciple;
        }

        public static Mock<IUserManager> CreateMockUserManager()
        {
            var userManager = new Mock<IUserManager>(MockBehavior.Strict);
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
                m.ConfirmEmailAsync(It.Is<string>(s => s == UnconfirmedGoodId),
                                    It.Is<string>(s => s == GoodToken)))
                                    .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(m =>
                m.ConfirmEmailAsync(It.Is<string>(s => s == UnconfirmedGoodId),
                                    It.Is<string>(s => s == BadToken)))
                                    .ReturnsAsync(new IdentityResult("Bad Token"));

            userManager.Setup(m =>
                m.ConfirmEmailAsync(It.Is<string>(s => s == BadId),
                                    It.IsAny<string>()))
                                    .Throws(new InvalidOperationException());

            userManager.Setup(m =>
                m.FindByNameAsync(It.Is<string>(s => s == ConfirmedGoodUsername)))
                                .ReturnsAsync(ConfirmedGoodUser);
            userManager.Setup(m =>
                m.FindByNameAsync(It.Is<string>(s => s == UnconfirmedGoodUsername)))
                                .ReturnsAsync(UnconfirmedGoodUser);
            userManager.Setup(m =>
                m.FindByNameAsync(It.Is<string>(s => s == BadUsername)))
                                .ReturnsAsync(null);

            userManager.Setup(m =>
                m.FindByIdAsync(It.Is<string>(s => s == ConfirmedGoodId)))
                                .ReturnsAsync(ConfirmedGoodUser);

            userManager.Setup(m =>
                m.IsEmailConfirmedAsync(It.Is<string>(s => s == ConfirmedGoodId)))
                                .ReturnsAsync(true);
            userManager.Setup(m =>
                m.IsEmailConfirmedAsync(It.Is<string>(s => s == UnconfirmedGoodId)))
                                .ReturnsAsync(false);

            userManager.Setup(m =>
                m.GeneratePasswordResetEmailAsync(It.IsAny<UrlHelper>(), It.Is<string>(s => s == ConfirmedGoodId)))
                                .Returns(Task.FromResult<object>(null));

            userManager.Setup(m =>
                m.ResetPasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == GoodToken), It.Is<string>(s => s == ConfirmedGoodPassword)))
                .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(m =>
                m.ResetPasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == GoodToken), It.Is<string>(s => s == BadPassword)))
                .ReturnsAsync(new IdentityResult("Password isn't good enough."));
            userManager.Setup(m =>
                m.ResetPasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == BadToken), It.Is<string>(s => s == ConfirmedGoodPassword)))
                .ReturnsAsync(new IdentityResult("Bad Token."));

            userManager.Setup(m =>
                m.ChangePasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == UnconfirmedGoodPassword), It.Is<string>(s => s == ConfirmedGoodPassword)))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(m =>
                m.ChangePasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == BadPassword), It.Is<string>(s => s == ConfirmedGoodPassword)))
                .ReturnsAsync(new IdentityResult("Old password was wrong."));
            userManager.Setup(m =>
                m.ChangePasswordAsync(It.Is<string>(s => s == ConfirmedGoodId), It.Is<string>(s => s == UnconfirmedGoodPassword), It.Is<string>(s => s == BadPassword)))
                .ReturnsAsync(new IdentityResult("New password isn't good enough."));

            return userManager;
        }

        public static void Validate(object model, ApiController controller)
        {
            Validate(model, controller.ModelState.AddModelError);
        }

        public static void Validate(object model, Controller controller)
        {
            Validate(model, controller.ModelState.AddModelError);
        }

        private static void Validate(object model, Action<string, string> storeResult)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);

            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);

            foreach (var v in results)
            {
                if (!v.MemberNames.Any())
                {
                    storeResult("model", v.ErrorMessage);
                }
                else
                {
                    foreach (var m in v.MemberNames)
                        storeResult(m, v.ErrorMessage);
                }
            }
        }
    }
}