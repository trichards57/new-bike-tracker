using BikeTracker.Controllers;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Xunit;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ControlPanelControllerTests
    {
        [Fact]
        public void Home()
        {
            var controller = new ControlPanelController();

            var res = controller.Home();

            Assert.NotNull(res);
        }

        [Fact]
        public void Index()
        {
            var controller = new ControlPanelController();

            var res = controller.Index();

            var view = res as ViewResult;

            Assert.NotNull(view);
        }
    }
}