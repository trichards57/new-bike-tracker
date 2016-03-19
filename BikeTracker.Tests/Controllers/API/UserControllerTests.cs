using BikeTracker.Controllers.API;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Tests.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.OData;

namespace BikeTracker.Tests.Controllers.API
{
    [TestClass]
    public class UserControllerTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly List<ApplicationUser> TestUsers;
        private readonly List<ApplicationRole> TestRoles;
        private readonly string TestGoodEmail;
        private readonly string TestBadEmail;
        private readonly ApplicationUser TestUser;
        private IEnumerable<string> TestRoleResult;
        private readonly ApplicationRole GoodRole;
        private readonly ApplicationRole BadRole;
        private readonly string BadUserId;
        private const string DefaultRole = "Normal";

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

        private UserController CreateController()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var controller = new UserController(userManager.Object, roleManager.Object);

            return controller;
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
        public async Task GetSingleUserBadId()
        {
            var controller = CreateController();

            var role = TestRoles.First();
            var res = (await controller.Get(BadUserId)).Queryable;

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Any());
        }

        [TestMethod]
        public async Task PutUserGoodData()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var model = new UserAdminViewModel
            {
                Id = TestUser.Id,
                EmailAddress = TestGoodEmail,
                Role = GoodRole.Name,
                RoleId = GoodRole.Id,
                RoleDisplayName = GoodRole.DisplayName,
                UserName = TestGoodEmail
            };

            var res = await controller.Put(TestUser.Id, model);

            Assert.IsInstanceOfType(res, typeof(OkResult));

            userManager.Verify(u => u.SetEmailAsync(TestUser.Id, TestGoodEmail));
            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id));
            userManager.Verify(u => u.RemoveFromRolesAsync(TestUser.Id, It.Is<string[]>(s => s.SequenceEqual(TestRoleResult))));
            userManager.Verify(u => u.AddToRoleAsync(TestUser.Id, GoodRole.Name));
        }

        [TestMethod]
        public async Task PutUserGoodEmail()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var role = TestRoles.First();

            var model = new UserAdminViewModel
            {
                Id = TestUser.Id,
                EmailAddress = TestGoodEmail,
                Role = role.Name,
                RoleId = role.Id,
                RoleDisplayName = role.DisplayName,
                UserName = TestGoodEmail
            };

            var res = await controller.Put(TestUser.Id, model);

            Assert.IsInstanceOfType(res, typeof(OkResult));

            userManager.Verify(u => u.SetEmailAsync(TestUser.Id, TestGoodEmail));
            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id));
        }

        [TestMethod]
        public async Task PutUserBadEmail()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var role = TestRoles.First();

            var model = new UserAdminViewModel
            {
                Id = TestUser.Id,
                EmailAddress = TestBadEmail,
                Role = role.Name,
                RoleId = role.Id,
                RoleDisplayName = role.DisplayName,
                UserName = TestGoodEmail
            };

            var res = await controller.Put(TestUser.Id, model);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));

            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id), Times.Never);
        }

        [TestMethod]
        public async Task PutUserBadUser()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var role = TestRoles.First();

            var model = new UserAdminViewModel
            {
                Id = BadUserId,
                EmailAddress = TestGoodEmail,
                Role = role.Name,
                RoleId = role.Id,
                RoleDisplayName = role.DisplayName,
                UserName = TestGoodEmail
            };

            var res = await controller.Put(BadUserId, model);

            Assert.IsInstanceOfType(res, typeof(NotFoundResult));

            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id), Times.Never);
        }

        [TestMethod]
        public async Task PutUserGoodRole()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var model = new UserAdminViewModel
            {
                Id = TestUser.Id,
                EmailAddress = TestUser.Email,
                Role = GoodRole.Name,
                RoleId = GoodRole.Id,
                RoleDisplayName = GoodRole.DisplayName,
                UserName = TestUser.UserName
            };

            var res = await controller.Put(TestUser.Id, model);

            Assert.IsInstanceOfType(res, typeof(OkResult));

            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id), Times.Never);
            userManager.Verify(u => u.RemoveFromRolesAsync(TestUser.Id, It.Is<string[]>(s => s.SequenceEqual(TestRoleResult))));
            userManager.Verify(u => u.AddToRoleAsync(TestUser.Id, GoodRole.Name));
        }

        [TestMethod]
        public async Task RegisterGoodData()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;


            var parameters = new ODataActionParameters();
            parameters.Add("role", GoodRole.Name);
            parameters.Add("email", TestGoodEmail);

            var res = await controller.Register(parameters);

            userManager.Verify(u => u.CreateAsync(It.Is<ApplicationUser>(a => a.Email == TestGoodEmail && a.UserName == TestGoodEmail && a.MustResetPassword), It.IsNotNull<string>()));
            userManager.Verify(u => u.AddToRoleAsync(TestUser.Id, GoodRole.Name));
            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>()));

            Assert.IsInstanceOfType(res, typeof(OkResult));
        }

        [TestMethod]
        public async Task RegisterDuplicateEmail()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;


            var parameters = new ODataActionParameters();
            parameters.Add("role", GoodRole.Name);
            parameters.Add("email", TestUser.Email);

            var res = await controller.Register(parameters);
            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }


        [TestMethod]
        public async Task RegisterBadEmail()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;


            var parameters = new ODataActionParameters();
            parameters.Add("role", GoodRole.Name);
            parameters.Add("email", TestBadEmail);

            var res = await controller.Register(parameters);

            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>()), Times.Never);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task RegisterEmptyEmail()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;


            var parameters = new ODataActionParameters();
            parameters.Add("role", GoodRole.Name);
            parameters.Add("email", string.Empty);

            var res = await controller.Register(parameters);

            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>()), Times.Never);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task RegisterNullEmail()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;


            var parameters = new ODataActionParameters();
            parameters.Add("role", GoodRole.Name);
            parameters.Add("email", null);

            var res = await controller.Register(parameters);

            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>()), Times.Never);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task RegisterBadRole()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var parameters = new ODataActionParameters();
            parameters.Add("role", BadRole.Name);
            parameters.Add("email", TestGoodEmail);

            var res = await controller.Register(parameters);

            userManager.Verify(u => u.CreateAsync(It.Is<ApplicationUser>(a => a.Email == TestGoodEmail && a.UserName == TestGoodEmail && a.MustResetPassword), It.IsNotNull<string>()));
            userManager.Verify(u => u.AddToRoleAsync(TestUser.Id, DefaultRole));
            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>()));

            Assert.IsInstanceOfType(res, typeof(OkResult));
        }

        [TestMethod]
        public async Task RegisterEmptyRole()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var parameters = new ODataActionParameters();
            parameters.Add("role", string.Empty);
            parameters.Add("email", TestGoodEmail);

            var res = await controller.Register(parameters);

            userManager.Verify(u => u.CreateAsync(It.Is<ApplicationUser>(a => a.Email == TestGoodEmail && a.UserName == TestGoodEmail && a.MustResetPassword), It.IsNotNull<string>()));
            userManager.Verify(u => u.AddToRoleAsync(TestUser.Id, DefaultRole));
            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>()));

            Assert.IsInstanceOfType(res, typeof(OkResult));
        }

        [TestMethod]
        public async Task RegisterNullRole()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var configuration = new Mock<HttpConfiguration>();
            var controller = new UserController(userManager.Object, roleManager.Object);
            controller.Configuration = configuration.Object;

            var request = new HttpRequest("", "http://localhost", "");
            var context = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current = context;

            var parameters = new ODataActionParameters();
            parameters.Add("role", null);
            parameters.Add("email", TestGoodEmail);

            var res = await controller.Register(parameters);

            userManager.Verify(u => u.CreateAsync(It.Is<ApplicationUser>(a => a.Email == TestGoodEmail && a.UserName == TestGoodEmail && a.MustResetPassword), It.IsNotNull<string>()));
            userManager.Verify(u => u.AddToRoleAsync(TestUser.Id, DefaultRole));
            userManager.Verify(u => u.GenerateEmailConfirmationEmailAsync(It.IsNotNull<UrlHelper>(), TestUser.Id, It.IsNotNull<string>()));

            Assert.IsInstanceOfType(res, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteGoodUser()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var controller = new UserController(userManager.Object, roleManager.Object);

            var res = await controller.Delete(TestUser.Id);

            userManager.Verify(u => u.DeleteAsync(TestUser));

            Assert.IsInstanceOfType(res, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task DeleteBadUser()
        {
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var controller = new UserController(userManager.Object, roleManager.Object);

            var res = await controller.Delete(BadUserId);

            Assert.IsInstanceOfType(res, typeof(StatusCodeResult));
        }
    }
}
