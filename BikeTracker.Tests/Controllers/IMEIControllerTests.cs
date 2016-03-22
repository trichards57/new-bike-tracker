using BikeTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [TestClass]
    public class IMEIControllerTests
    {
        [TestMethod]
        public void EditForm()
        {
            var controller = new IMEIController();

            var res = controller.EditForm();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }

        [TestMethod]
        public void Home()
        {
            var controller = new IMEIController();

            var res = controller.Home();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }
    }
}