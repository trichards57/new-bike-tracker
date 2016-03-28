using BikeTracker.Controllers.API;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.OData;

namespace BikeTracker.Tests.Controllers.API
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserControllerTests
    {
        private const string DefaultRole = "Normal";
        private readonly ApplicationRole BadRole;
        private readonly string BadUserId;
        private readonly Fixture Fixture = new Fixture();
        private readonly ApplicationRole GoodRole;
        private readonly string TestBadEmail;
        private readonly string TestGoodEmail;
        private readonly List<ApplicationRole> TestRoles;
        private readonly ApplicationUser TestUser;
        private readonly string TestUsername;
        private readonly List<ApplicationUser> TestUsers;
        private IEnumerable<string> TestRoleResult;

        public UserControllerTests()
        {
            TestUsers = new List<ApplicationUser>(Fixture.CreateMany<ApplicationUser>());

            foreach (var u in TestUsers)
            {
                u.Email = Fixture.Create<MailAddress>().Address;
            }

            TestRoles = new List<ApplicationRole>(Fixture.CreateMany<ApplicationRole>());
            TestUser = TestUsers.First();
            TestRoleResult = TestRoles.Select(r => r.Name).Take(1);
            GoodRole = TestRoles.Last();
            BadRole = Fixture.Create<ApplicationRole>();
            TestGoodEmail = Fixture.Create<MailAddress>().Address;
            TestBadEmail = Fixture.Create<string>();
            BadUserId = Fixture.Create<string>();
            TestUsername = Fixture.Create<string>();
        }

        [TestMethod]
        public async Task DeleteBadUser()
        {
            await DeleteUser(BadUserId, expectSuccess: false);
        }

        [TestMethod]
        public async Task DeleteGoodUser()
        {
            await DeleteUser(TestUser.Id, testUser: TestUser);
        }

        [TestMethod]
        public async Task GetSingleUserBadId()
        {
            var controller = CreateController();

            var role = TestRoles.First();
            var res = (await controller.Get(BadUserId)).Queryable;

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Any());
        }

        [TestMethod]
        public async Task GetSingleUserGoodId()
        {
            var controller = CreateController();

            var role = TestRoles.First();
            var user = (await controller.Get(TestUsers.First().Id)).Queryable.First();

            var original = TestUsers.First(us => us.Id == user.Id);

            Assert.AreEqual(original.Email, user.EmailAddress);
            Assert.AreEqual(original.UserName, user.UserName);
            Assert.AreEqual(role.Name, user.Role);
            Assert.AreEqual(role.DisplayName, user.RoleDisplayName);
            Assert.AreEqual(role.Id, user.RoleId);
        }

        [TestMethod]
        public async Task GetUser()
        {
            var controller = CreateController();

            var users = await controller.GetUser();
            var role = TestRoles.First();

            foreach (var u in users)
            {
                var original = TestUsers.First(us => us.Id == u.Id);

                Assert.AreEqual(original.Email, u.EmailAddress);
                Assert.AreEqual(original.UserName, u.UserName);
                Assert.AreEqual(role.Name, u.Role);
                Assert.AreEqual(role.DisplayName, u.RoleDisplayName);
                Assert.AreEqual(role.Id, u.RoleId);
            }
        }

        [TestMethod]
        public async Task PutUserBadEmail()
        {
            await PutUser(TestUser.Id, TestBadEmail, GoodRole, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task PutUserBadUser()
        {
            await PutUser(BadUserId, TestGoodEmail, GoodRole, expectedResult: ResultType.NotFoundError);
        }

        [TestMethod]
        public async Task PutUserGoodData()
        {
            await PutUser(TestUser.Id, TestGoodEmail, GoodRole, TestRoleResult);
        }

        [TestMethod]
        public async Task PutUserGoodEmailOnly()
        {
            await PutUser(TestUser.Id, TestGoodEmail, TestRoles.First(), TestRoleResult, changeRole: false);
        }

        [TestMethod]
        public async Task PutUserGoodRoleOnly()
        {
            await PutUser(TestUser.Id, TestUser.Email, GoodRole, TestRoleResult, changeEmail: false);
        }

        [TestMethod]
        public async Task PutUserNoChanges()
        {
            await PutUser(TestUser.Id, TestUser.Email, TestRoles.First(), TestRoleResult, changeEmail: false, changeRole: false);
        }

        [TestMethod]
        public async Task RegisterBadEmail()
        {
            await RegisterUser(GoodRole.Name, TestBadEmail, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task RegisterBadRole()
        {
            await RegisterUser(BadRole.Name, TestGoodEmail, TestUser.Id, useDefaultRole: true);
        }

        [TestMethod]
        public async Task RegisterDuplicateEmail()
        {
            await RegisterUser(GoodRole.Name, TestUser.Email, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task RegisterEmptyEmail()
        {
            await RegisterUser(GoodRole.Name, string.Empty, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task RegisterEmptyRole()
        {
            await RegisterUser(string.Empty, TestGoodEmail, TestUser.Id, useDefaultRole: true);
        }

        [TestMethod]
        public async Task RegisterGoodData()
        {
            await RegisterUser(GoodRole.Name, TestGoodEmail, TestUser.Id);
        }

        [TestMethod]
        public async Task RegisterNullEmail()
        {
            await RegisterUser(GoodRole.Name, null, expectedResult: ResultType.ModelError);
        }

        [TestMethod]
        public async Task RegisterNullRole()
        {
            await RegisterUser(null, TestGoodEmail, TestUser.Id, useDefaultRole: true);
        }

        private bool CheckUpdateProperties(IEnumerable<string> properties, bool changeRole, bool changeEmail)
        {
            var res = true;
            if (changeEmail)
                res &= properties.Contains("Email");
            else
                res &= !properties.Contains("Email");

            if (changeRole)
                res &= properties.Contains("Role");
            else
                res &= !properties.Contains("Role");

            return res;
        }

        private UserController CreateController()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var logService = GetMockLogService();
            var controller = new UserController(userManager.Object, roleManager.Object, logService.Object);

            return controller;
        }

        private Mock<IPrincipal> CreateMockPrincipal()
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, TestUsername));

            var mockPrinciple = new Mock<IPrincipal>();
            mockPrinciple.SetupGet(i => i.Identity).Returns(identity);

            return mockPrinciple;
        }

        private async Task DeleteUser(string id, bool expectSuccess = true, ApplicationUser testUser = null)
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var logService = GetMockLogService();
            var controller = new UserController(userManager.Object, roleManager.Object, logService.Object);

            var principal = CreateMockPrincipal();
            controller.User = principal.Object;

            var res = await controller.Delete(id);

            if (expectSuccess)
            {
                userManager.Verify(u => u.DeleteAsync(testUser));
                logService.Verify(l => l.LogUserDeleted(TestUsername, testUser.UserName));
            }

            Assert.IsInstanceOfType(res, typeof(StatusCodeResult));
        }

        private Mock<ILogService> GetMockLogService()
        {
            var service = new Mock<ILogService>(MockBehavior.Strict);

            service.Setup(l => l.LogUserCreated(TestUsername, TestGoodEmail)).Returns(Task.FromResult<object>(null));
            service.Setup(l => l.LogUserUpdated(TestUsername, It.IsAny<IEnumerable<string>>())).Returns(Task.FromResult<object>(null));
            service.Setup(l => l.LogUserDeleted(TestUsername, It.IsAny<string>())).Returns(Task.FromResult<object>(null));

            return service;
        }

        private Mock<IRoleManager> GetMockRoleManager()
        {
            var roleManager = new Mock<IRoleManager>(MockBehavior.Strict);

            var role = TestRoles.First();

            roleManager.Setup(r => r.FindByNameAsync(role.Name)).ReturnsAsync(role);
            roleManager.Setup(r => r.RoleExistsAsync(GoodRole.Name)).ReturnsAsync(true);
            roleManager.Setup(r => r.RoleExistsAsync(BadRole.Name)).ReturnsAsync(false);
            roleManager.Setup(r => r.RoleExistsAsync(DefaultRole)).ReturnsAsync(true);

            return roleManager;
        }

        private Mock<IUserManager> GetMockUserManager()
        {
            var userManager = new Mock<IUserManager>(MockBehavior.Strict);
            var userData = TestUsers.AsQueryable();

            var userSet = new Mock<DbSet<ApplicationUser>>();
            userSet.As<IDbAsyncEnumerable<ApplicationUser>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<ApplicationUser>(userData.GetEnumerator()));
            userSet.As<IQueryable<ApplicationUser>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<ApplicationUser>(userData.Provider));

            userManager.SetupGet(u => u.Users).Returns(userSet.Object);
            userManager.Setup(u => u.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(TestRoleResult.ToList());
            userManager.Setup(u => u.FindByIdAsync(TestUser.Id)).ReturnsAsync(TestUser);
            userManager.Setup(u => u.FindByIdAsync(BadUserId)).ReturnsAsync(null);
            userManager.Setup(u => u.FindByEmailAsync(TestGoodEmail)).ReturnsAsync(TestUser);

            userManager.Setup(u => u.SetEmailAsync(TestUser.Id, TestGoodEmail)).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(u => u.SetEmailAsync(TestUser.Id, TestBadEmail)).ReturnsAsync(new IdentityResult("Bad Email Address"));
            userManager.Setup(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id)).Returns(Task.FromResult<object>(null));
            userManager.Setup(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>())).Returns(Task.FromResult<object>(null));
            userManager.Setup(u => u.RemoveFromRolesAsync(TestUser.Id, It.Is<string[]>(s => s.SequenceEqual(TestRoleResult)))).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(u => u.AddToRoleAsync(TestUser.Id, GoodRole.Name)).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(u => u.AddToRoleAsync(TestUser.Id, DefaultRole)).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(u => u.AddToRoleAsync(TestUser.Id, BadRole.Name)).ReturnsAsync(new IdentityResult("Bad Role"));
            userManager.Setup(u => u.IsInRoleAsync(TestUser.Id, TestRoleResult.First())).ReturnsAsync(true);
            userManager.Setup(u => u.IsInRoleAsync(TestUser.Id, GoodRole.Name)).ReturnsAsync(false);
            userManager.Setup(u => u.IsInRoleAsync(TestUser.Id, BadRole.Name)).ReturnsAsync(false);
            userManager.Setup(u => u.CreateAsync(It.Is<ApplicationUser>(a => a.Email == TestGoodEmail && a.UserName == TestGoodEmail && a.MustResetPassword), It.IsNotNull<string>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(u => u.CreateAsync(It.Is<ApplicationUser>(a => a.Email == TestUser.Email), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed("Already Used"));
            userManager.Setup(u => u.DeleteAsync(TestUser)).ReturnsAsync(IdentityResult.Success);

            var passwordValidator = new Mock<IIdentityValidator<string>>();
            passwordValidator.SetupSequence(p => p.ValidateAsync(It.IsNotNull<string>())).Returns(Task.FromResult(new IdentityResult("Wrong Password")))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManager.SetupGet(u => u.PasswordValidator).Returns(passwordValidator.Object);

            return userManager;
        }

        private async Task PutUser(string id, string email, ApplicationRole role, IEnumerable<string> oldRoles = null, bool changeRole = true, bool changeEmail = true, ResultType expectedResult = ResultType.Success)
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var logService = GetMockLogService();
            var controller = new UserController(userManager.Object, roleManager.Object, logService.Object);
            controller.Configuration = configuration.Object;

            var principal = CreateMockPrincipal();
            controller.User = principal.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var model = new UserAdminViewModel
            {
                Id = id,
                EmailAddress = email,
                Role = role.Name,
                RoleId = role.Id,
                RoleDisplayName = role.DisplayName,
                UserName = email
            };

            var res = await controller.Put(id, model);

            switch (expectedResult)
            {
                case ResultType.Success:
                    Assert.IsInstanceOfType(res, typeof(OkResult));

                    if (changeEmail)
                    {
                        userManager.Verify(u => u.SetEmailAsync(id, email));
                        userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), id));
                    }
                    else
                    {
                        userManager.Verify(u => u.SetEmailAsync(id, email), Times.Never);
                        userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), id), Times.Never);
                    }

                    if (changeRole)
                    {
                        userManager.Verify(u => u.RemoveFromRolesAsync(id, It.Is<string[]>(s => s.SequenceEqual(oldRoles))));
                        userManager.Verify(u => u.AddToRoleAsync(id, role.Name));
                    }
                    else
                    {
                        userManager.Verify(u => u.RemoveFromRolesAsync(id, It.Is<string[]>(s => s.SequenceEqual(oldRoles))), Times.Never);
                        userManager.Verify(u => u.AddToRoleAsync(id, role.Name), Times.Never);
                    }

                    if (changeEmail || changeRole)
                        logService.Verify(l => l.LogUserUpdated(TestUsername, It.Is<IEnumerable<string>>(s => CheckUpdateProperties(s, changeRole, changeEmail))));
                    else
                        logService.Verify(l => l.LogUserUpdated(TestUsername, It.IsAny<IEnumerable<string>>()), Times.Never);

                    break;

                case ResultType.ModelError:
                    Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
                    userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), id), Times.Never);
                    break;

                case ResultType.NotFoundError:
                    Assert.IsInstanceOfType(res, typeof(NotFoundResult));
                    userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), id), Times.Never);
                    break;
            }
        }

        private async Task RegisterUser(string role, string email, string id = null, ResultType expectedResult = ResultType.Success, bool useDefaultRole = false)
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var logService = GetMockLogService();
            var controller = new UserController(userManager.Object, roleManager.Object, logService.Object);
            controller.Configuration = configuration.Object;

            var principal = CreateMockPrincipal();
            controller.User = principal.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var parameters = new ODataActionParameters();
            parameters.Add("role", role);
            parameters.Add("email", email);

            var res = await controller.Register(parameters);

            var expectedRole = role;
            if (useDefaultRole)
                expectedRole = DefaultRole;

            switch (expectedResult)
            {
                case ResultType.Success:
                    userManager.Verify(u => u.CreateAsync(It.Is<ApplicationUser>(a => a.Email == email && a.UserName == email && a.MustResetPassword), It.IsNotNull<string>()));
                    userManager.Verify(u => u.AddToRoleAsync(id, expectedRole));
                    userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), id, It.IsNotNull<string>()));

                    logService.Verify(l => l.LogUserCreated(TestUsername, email));

                    Assert.IsInstanceOfType(res, typeof(OkResult));
                    break;

                case ResultType.ModelError:
                    userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsAny<UrlHelper>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                    logService.Verify(l => l.LogUserCreated(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                    Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
                    break;
            }
        }
    }
}