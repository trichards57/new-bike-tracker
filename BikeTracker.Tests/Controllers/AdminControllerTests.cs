using BikeTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        [TestMethod]
        public void Home()
        {
            var controller = new AdminController();

            var res = controller.Home();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }

        [TestMethod]
        public void EditForm()
        {
            var controller = new AdminController();

            var res = controller.EditForm();

            var partialView = res as PartialViewResult;

            Assert.IsNotNull(partialView);
        }
    }
}
