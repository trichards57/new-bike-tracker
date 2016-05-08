using BikeTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ControlPanelControllerTests
    {
        [TestMethod]
        public void Home()
        {
            var controller = new ControlPanelController();

            var res = controller.Home();

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Index()
        {
            var controller = new ControlPanelController();

            var res = controller.Index();

            var view = res as ViewResult;

            Assert.IsNotNull(view);
        }
    }
}