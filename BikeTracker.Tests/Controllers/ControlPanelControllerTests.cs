using BikeTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [TestClass]
    public class ControlPanelControllerTests
    {
        [TestMethod]
        public void Index()
        {
            var controller = new ControlPanelController();

            var res = controller.Index();

            var view = res as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void Home()
        {
            var controller = new ControlPanelController();

            var res = controller.Home();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }
    }
}
