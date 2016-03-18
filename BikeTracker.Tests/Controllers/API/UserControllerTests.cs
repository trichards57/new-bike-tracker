using BikeTracker.Controllers.API;
using BikeTracker.Models.Contexts;
using BikeTracker.Models.IdentityModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Tests.Controllers.API
{
    [TestClass]
    public class UserControllerTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly List<ApplicationUser> Users;

        public UserControllerTests()
        {
            Users = new List<ApplicationUser>(Fixture.CreateMany<ApplicationUser>());
        }

        private UserController CreateController()
        {
            var userManager = new Mock<IUserManager>(MockBehavior.Strict);
            var roleManager = new Mock<IRoleManager>(MockBehavior.Strict);

            var controller = new UserController(userManager.Object, roleManager.Object);

            return controller;
        }

        [TestMethod]
        public async Task GetUser()
        {
            var controller = CreateController();

            var users = await controller.GetUser();

            foreach (var u in users)
            {
                var original = Users.First(us => us.Id == u.Id);
                
                Assert.AreEqual(original.Email, u.EmailAddress);
                Assert.AreEqual(original.UserName, u.UserName);
            }
        }
    }
}
