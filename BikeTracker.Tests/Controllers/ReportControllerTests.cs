using BikeTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReportControllerTests
    {
        [TestMethod]
        public void Home()
        {
            var controller = new ReportController();

            var res = controller.Home();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }

        [TestMethod]
        public void Locations()
        {
            var controller = new ReportController();

            var res = controller.Locations();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }

        [TestMethod]
        public void Rates()
        {
            var controller = new ReportController();

            var res = controller.Rates();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }

        [TestMethod]
        public void UserActivity()
        {
            var controller = new ReportController();

            var res = controller.UserActivity();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }
    }
}