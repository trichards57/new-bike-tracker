using BikeTracker.Models;
using BikeTracker.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [TestClass]
    public class ManageControllerTests
    {
        [TestMethod]
        public void Index()
        {
            var controller = AccountMockHelpers.CreateManageController();

            var result = controller.Index(null);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void StartChangePassword()
        {
            var controller = AccountMockHelpers.CreateManageController();

            var result = controller.ChangePassword();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ChangePasswordGoodData()
        {
            var controller = AccountMockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = AccountMockHelpers.UnconfirmedGoodPassword,
                NewPassword = AccountMockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = AccountMockHelpers.ConfirmedGoodPassword
            };

            var result = await controller.ChangePassword(model);

            var view = result as RedirectToRouteResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [TestMethod]
        public async Task ChangePasswordWrongPassword()
        {
            var controller = AccountMockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = AccountMockHelpers.BadPassword,
                NewPassword = AccountMockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = AccountMockHelpers.ConfirmedGoodPassword
            };

            var result = await controller.ChangePassword(model);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ChangePasswordBadNewPassword()
        {
            var controller = AccountMockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = AccountMockHelpers.UnconfirmedGoodPassword,
                NewPassword = AccountMockHelpers.BadPassword,
                ConfirmPassword = AccountMockHelpers.BadPassword
            };

            var result = await controller.ChangePassword(model);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ChangePasswordNewPasswordMismatch()
        {
            var controller = AccountMockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = AccountMockHelpers.UnconfirmedGoodPassword,
                NewPassword = AccountMockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = AccountMockHelpers.BadPassword
            };

            AccountMockHelpers.Validate(model, controller);

            var result = await controller.ChangePassword(model);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }
    }
}
