using BikeTracker.Controllers;
using BikeTracker.Models.AccountViewModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
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
    [ExcludeFromCodeCoverage]
    public class ManageControllerTests
    {
        [Fact]
        public async Task ChangePasswordBadNewPassword()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.UnconfirmedGoodPassword, MockHelpers.BadPassword, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, false, true, false);
        }

        [Fact]
        public async Task ChangePasswordGoodData()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.UnconfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername);
        }

        [Fact]
        public async Task ChangePasswordGoodDataWithDependencyResolver()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.UnconfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername, true);
        }

        [Fact]
        public async Task ChangePasswordNewPasswordMismatch()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.UnconfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodUsername, false, false, false);
        }

        [Fact]
        public async Task ChangePasswordWrongPassword()
        {
            await ChangePassword(MockHelpers.ConfirmedGoodId, MockHelpers.BadPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodPassword, MockHelpers.ConfirmedGoodUsername, false, true, false);
        }

        [Fact]
        public void Index()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.Index(null);

            var view = result as ViewResult;

            Assert.NotNull(view);
        }

        [Fact]
        public void IndexError()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.Index(ManageController.ManageMessageId.Error);

            var view = result as ViewResult;

            Assert.NotNull(view);
        }

        [Fact]
        public void IndexPasswordSuccess()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.Index(ManageController.ManageMessageId.ChangePasswordSuccess);

            var view = result as ViewResult;

            Assert.NotNull(view);
        }

        [Fact]
        public void StartChangePassword()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.ChangePassword();

            var view = result as ViewResult;

            Assert.NotNull(view);
        }

        private async Task ChangePassword(string userId, string oldPassword, string newPassword, string confirmPassword, string username, bool useDependencyService = false, bool shouldReset = true, bool shouldReportSuccess = true)
        {
            var userManager = MockHelpers.CreateMockUserManager();
            var signInManager = MockHelpers.CreateMockSignInManager();
            var httpContext = MockHelpers.CreateMockHttpContext();
            var logService = CreateMockLogService();

            var container = new UnityContainer();

            container.RegisterInstance(logService.Object);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            var controller = new ManageController(userManager.Object, signInManager.Object, useDependencyService ? null : logService.Object);
            var context = new ControllerContext { HttpContext = httpContext.Object };
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
                Assert.IsType<RedirectToRouteResult>(result);
                var view = (RedirectToRouteResult)result;
                Assert.Equal("Index", view.RouteValues["action"]);
            }
            else
                Assert.IsType<ViewResult>(result);

            if (shouldReset)
                userManager.Verify(u => u.ChangePasswordAsync(userId, oldPassword, newPassword));
            else
                userManager.Verify(u => u.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            if (shouldReportSuccess && shouldReset)
                logService.Verify(s => s.LogUserUpdated(username, username, It.Is<IEnumerable<string>>(i => i.Single() == "Password")));
            else
                logService.Verify(s => s.LogUserUpdated(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        private Mock<ILogService> CreateMockLogService()
        {
            var result = new Mock<ILogService>(MockBehavior.Strict);

            result.Setup(s => s.LogUserUpdated(MockHelpers.ConfirmedGoodUsername, MockHelpers.ConfirmedGoodUsername, It.Is<IEnumerable<string>>(i => i.Single() == "Password"))).Returns(Task.FromResult<object>(null));

            return result;
        }
    }
}