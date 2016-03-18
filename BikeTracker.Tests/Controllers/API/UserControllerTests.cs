using BikeTracker.Controllers.API;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Tests.Controllers.API
{
    [TestClass]
    public class UserControllerTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly List<ApplicationUser> TestUsers;
        private readonly List<ApplicationRole> TestRoles;

        public UserControllerTests()
        {
            TestUsers = new List<ApplicationUser>(Fixture.CreateMany<ApplicationUser>());
            TestRoles = new List<ApplicationRole>(Fixture.CreateMany<ApplicationRole>());
        }

        private UserController CreateController()
        {
            var userManager = new Mock<IUserManager>(MockBehavior.Strict);
            var roleManager = new Mock<IRoleManager>(MockBehavior.Strict);

            var userData = TestUsers.AsQueryable();

            var userSet = new Mock<DbSet<ApplicationUser>>();
            userSet.As<IDbAsyncEnumerable<ApplicationUser>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<ApplicationUser>(userData.GetEnumerator()));
            userSet.As<IQueryable<ApplicationUser>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<ApplicationUser>(userData.Provider));

            userManager.SetupGet(u => u.Users).Returns(userSet.Object);
            userManager.Setup(u => u.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(TestRoles.Select(r=>r.Name).ToList());

            roleManager.Setup(r => r.FindByNameAsync(It.IsAny<string>())).Returns<string>(s => Task.FromResult(TestRoles.FirstOrDefault(r => r.Name == s)));

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
    }
}
