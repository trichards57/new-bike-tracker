using BikeTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [TestClass]
    public class DialogControllerTests
    {
        [TestMethod]
        public void DeleteForm()
        {
            var controller = new DialogController();

            var res = controller.DeleteForm();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }

        [TestMethod]
        public void ErrorForm()
        {
            var controller = new DialogController();

            var res = controller.ErrorForm();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }
    }
}