using BikeTracker.Controllers;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Xunit;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class HomeControllerTest
    {
        [Fact]
        public void Contact()
        {
            var controller = new HomeController();
            var result = controller.Contact() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void Index()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void Policies()
        {
            var controller = new HomeController();
            var result = controller.Policies() as ViewResult;
            Assert.NotNull(result);
        }
    }
}