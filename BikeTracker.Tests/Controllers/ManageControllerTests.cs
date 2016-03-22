using BikeTracker.Models.AccountViewModels;
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
        public async Task ChangePasswordBadNewPassword()
        {
            var controller = MockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = MockHelpers.UnconfirmedGoodPassword,
                NewPassword = MockHelpers.BadPassword,
                ConfirmPassword = MockHelpers.BadPassword
            };

            var result = await controller.ChangePassword(model);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ChangePasswordGoodData()
        {
            var controller = MockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = MockHelpers.UnconfirmedGoodPassword,
                NewPassword = MockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = MockHelpers.ConfirmedGoodPassword
            };

            var result = await controller.ChangePassword(model);

            var view = result as RedirectToRouteResult;

            Assert.IsNotNull(view);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [TestMethod]
        public async Task ChangePasswordNewPasswordMismatch()
        {
            var controller = MockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = MockHelpers.UnconfirmedGoodPassword,
                NewPassword = MockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = MockHelpers.BadPassword
            };

            MockHelpers.Validate(model, controller);

            var result = await controller.ChangePassword(model);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task ChangePasswordWrongPassword()
        {
            var controller = MockHelpers.CreateManageController();

            var model = new ChangePasswordViewModel
            {
                OldPassword = MockHelpers.BadPassword,
                NewPassword = MockHelpers.ConfirmedGoodPassword,
                ConfirmPassword = MockHelpers.ConfirmedGoodPassword
            };

            var result = await controller.ChangePassword(model);

            var view = result as ViewResult;

            Assert.IsNotNull(view);
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
        public void StartChangePassword()
        {
            var controller = MockHelpers.CreateManageController();

            var result = controller.ChangePassword();

            var view = result as ViewResult;

            Assert.IsNotNull(view);
        }
    }
}