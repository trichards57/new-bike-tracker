using BikeTracker.Controllers.API;
using BikeTracker.Models;
using BikeTracker.Models.Contexts;
using BikeTracker.Models.IdentityModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            var dataContext = new Mock<ApplicationDbContext>(MockBehavior.Strict);
            var userSet = new Mock<DbSet<ApplicationUser>>(MockBehavior.Strict);

            var userQueryable = Users.AsQueryable();

            userSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(userQueryable.Provider);
            userSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(userQueryable.Expression);
            userSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(userQueryable.ElementType);
            userSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(userQueryable.GetEnumerator());

            dataContext.SetupGet(d => d.Users).Returns(userSet.Object);
            dataContext.SetupGet(d => d.IMEIToCallsigns);
            dataContext.SetupGet(d => d.Landmarks);
            dataContext.SetupGet(d => d.LocationRecords);
            dataContext.SetupGet(d => d.RequireUniqueEmail);
            dataContext.SetupGet(d => d.Roles);

            var controller = new UserController(dataContext.Object);

            return controller;
        }

        [TestMethod]
        public void GetUser()
        {
            var controller = CreateController();

            var users = controller.GetUser();

            foreach (var u in users)
            {
                var original = Users.First(us => us.Id == u.Id);
                
                Assert.AreEqual(original.Email, u.EmailAddress);
                Assert.AreEqual(original.UserName, u.UserName);
            }
        }
    }
}
