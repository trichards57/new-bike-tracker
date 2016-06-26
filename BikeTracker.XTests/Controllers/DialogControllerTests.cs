using BikeTracker.Controllers;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Xunit;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class DialogControllerTests
    {
        [Fact]
        public void DeleteForm()
        {
            var controller = new DialogController();

            var res = controller.DeleteForm();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }

        [Fact]
        public void ErrorForm()
        {
            var controller = new DialogController();

            var res = controller.ErrorForm();

            var partialView = res as PartialViewResult;

            Assert.NotNull(partialView);
        }
    }
}