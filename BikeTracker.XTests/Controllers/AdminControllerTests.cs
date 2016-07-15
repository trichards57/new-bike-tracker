using BikeTracker.Controllers;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Xunit;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class AdminControllerTests
    {
        [Fact]
        public void EditForm()
        {
            var controller = new AdminController();

            var res = controller.EditForm();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }

        [Fact]
        public void Home()
        {
            var controller = new AdminController();

            var res = controller.Home();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }
    }
}