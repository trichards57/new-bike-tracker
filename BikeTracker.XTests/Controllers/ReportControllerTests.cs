using BikeTracker.Controllers;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Xunit;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ReportControllerTests
    {
        [Fact]
        public void Home()
        {
            var controller = new ReportController();

            var res = controller.Home();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }

        [Fact]
        public void Locations()
        {
            var controller = new ReportController();

            var res = controller.Locations();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }

        [Fact]
        public void Rates()
        {
            var controller = new ReportController();

            var res = controller.Rates();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }

        [Fact]
        public void UserActivity()
        {
            var controller = new ReportController();

            var res = controller.UserActivity();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }
    }
}