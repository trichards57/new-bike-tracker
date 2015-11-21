using BikeTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            var controller = new HomeController();
            var result = controller.Contact() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Policies()
        {
            var controller = new HomeController();
            var result = controller.Policies() as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
