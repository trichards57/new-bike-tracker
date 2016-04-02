using BikeTracker.Controllers;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ManageControllerTests
    {
        [TestMethod]
        public async Task ChangePasswordBadNewPassword()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.UnconfirmedGoodPassword, MockHelpers.BadPassword, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, true, false);
        }

        [TestMethod]
        public async Task ChangePasswordGoodData()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.UnconfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername);
        }

        [TestMethod]
        public async Task ChangePasswordNewPasswordMismatch()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.UnconfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, false, false);
        }

        [TestMethod]
        public async Task ChangePasswordWrongPassword()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername, true, false);
        }

        [TestMethod]
        public void Index()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.Index(null);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void IndexError()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.Index(ManageController.ManageMessageId.Error);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void IndexPasswordSuccess()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.Index(ManageController.ManageMessageId.ChangePasswordSuccess);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void StartChangePassword()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.ChangePassword();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        private async Task ChangePassword(string userId, string oldPassword, string newPassword, string confirmPassword, string username, bool shouldReset = true, bool shouldReportSuccess = true)
        {
            var userManager = MockHelpers.CreateMockUserManager();
            var signInManager = MockHelpers.CreateMockSignInManager();
            var httpContext = MockHelpers.CreateMockHttpContext();
            var logService = CreateMockLogService();

            var controller = new ManageController(userManager.Object, signInManager.Object, logService.Object);
            var context = new ControllerContext();
            context.HttpContext = httpContext.Object;
            controller.ControllerContext = context;

            var model = new ChangePasswordViewModel
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };

            MockHelpers.Validate(model, controller);

            var result = await controller.ChangePassword(model);

            if (shouldReportSuccess)
            {
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var view = (RedirectToRouteResult)result;
                Assert.AreEqual("Index", view.RouteValues["action"]);
            }
            else
                Assert.IsInstanceOfType(result, typeof(ViewResult));

            if (shouldReset)
                userManager.Verify(u => u.ChangePasswordAsync(userId, oldPassword, newPassword));
            else
                userManager.Verify(u => u.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            if (shouldReportSuccess && shouldReset)
                logService.Verify(s => s.LogUserUpdated(username, It.Is<IEnumerable<string>>(i => i.Single() == "Password")));
            else
                logService.Verify(s => s.LogUserUpdated(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        private Mock<ILogService> CreateMockLogService()
        {
            var result = new Mock<ILogService>(MockBehavior.Strict);

            result.Setup(s => s.LogUserUpdated(MockHelpers.ConfirmedGoodUsername, It.Is<IEnumerable<string>>(i => i.Single() == "Password"))).Returns(Task.FromResult<object>(null));

            return result;
        }
    }
}