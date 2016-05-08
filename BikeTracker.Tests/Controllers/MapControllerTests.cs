using BikeTracker.Controllers;
using BikeTracker.Models;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MapControllerTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly ClaimsIdentity Identity;
        private readonly IEnumerable<Landmark> Landmarks;
        private readonly Expression<Func<ILocationService, Task>> LocationRegisterExpression;
        private readonly IEnumerable<LocationRecord> Locations;
        private readonly string TestCallsign;
        private readonly DateTimeOffset TestDate;
        private readonly int TestId;
        private readonly string TestIMEI;
        private readonly string TestLandmark;
        private readonly decimal TestLatitude;
        private readonly decimal TestLongitude;
        private readonly string TestUsername;

        public MapControllerTests()
        {
            Locations = Fixture.CreateMany<LocationRecord>();
            Landmarks = Fixture.CreateMany<Landmark>();
            TestDate = Fixture.Create<DateTimeOffset>();
            TestLatitude = Fixture.Create<decimal>();
            TestLongitude = Fixture.Create<decimal>();
            TestIMEI = Fixture.Create<string>();
            TestLandmark = Fixture.Create<string>();
            TestCallsign = Fixture.Create<string>();
            TestId = Fixture.Create<int>();
            LocationRegisterExpression = (l => l.RegisterLocation(It.Is<string>(s => s == TestIMEI),
                It.Is<DateTimeOffset>(d => Math.Abs((d - TestDate).TotalSeconds) < 1),
                It.Is<DateTimeOffset>(d => Math.Abs((d - DateTimeOffset.Now).TotalSeconds) < 5),
                It.Is<decimal>(s => s == TestLatitude), It.Is<decimal>(s => s == TestLongitude)));

            TestUsername = Fixture.Create<string>();

            Identity = new ClaimsIdentity();
            Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, TestUsername));
        }

        [TestMethod]
        public async Task AddLandmarkEmptyName()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.AddLandmark(string.Empty, TestLatitude, TestLongitude);

            var view = res as HttpStatusCodeResult;

            Assert.IsNotNull(view);
            Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)view.StatusCode);
        }

        [TestMethod]
        public async Task AddLandmarkGoodData()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.AddLandmark(TestLandmark, TestLatitude, TestLongitude);

            locationService.Verify(l => l.RegisterLandmark(It.Is<string>(s => s == TestLandmark), It.Is<decimal>(s => s == TestLatitude), It.Is<decimal>(s => s == TestLongitude), null));

            var view = res as HttpStatusCodeResult;

            Assert.IsNotNull(view);
            Assert.AreEqual(HttpStatusCode.Created, (HttpStatusCode)view.StatusCode);
        }

        [TestMethod]
        public async Task AddLandmarkNoName()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.AddLandmark(null, TestLatitude, TestLongitude);

            var view = res as HttpStatusCodeResult;

            Assert.IsNotNull(view);
            Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)view.StatusCode);
        }

        [TestMethod]
        public async Task CheckInBadDate()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testDate, testTime);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInEmptyIMEI()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(string.Empty, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInGoodData()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testTime, testDate);

            locationService.Verify(LocationRegisterExpression);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoDate()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testTime, null);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoIMEI()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(null, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoLatitude()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, null, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoLongitude()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, null, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoTime()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, null, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        public Mock<HttpContextBase> CreateMockHttpContext()
        {
            var context = new Mock<HttpContextBase>(MockBehavior.Strict);

            context.SetupGet(h => h.User.Identity).Returns(Identity);

            return context;
        }

        [TestMethod]
        public async Task GetLandmarks()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();
            var logService = CreateMockLogService();

            var controller = new MapController(locationService.Object, imeiService.Object, logService.Object);

            var httpContext = CreateMockHttpContext();
            var context = new ControllerContext { HttpContext = httpContext.Object };

            controller.ControllerContext = context;

            var res = await controller.GetLandmarks();
            Assert.IsNotNull(res);

            var data = res.Data as IEnumerable<Landmark>;
            Assert.IsNotNull(data);

            var enumerable = data as IList<Landmark> ?? data.ToList();
            foreach (var d in enumerable)
            {
                var original = Landmarks.First(l => l.Id == d.Id);
                Assert.AreEqual(original.Name, d.Name);
                Assert.AreEqual(original.Latitude, d.Latitude);
                Assert.AreEqual(original.Longitude, d.Longitude);
                Assert.AreEqual(original.Expiry, d.Expiry);
            }

            Assert.AreEqual(Locations.Count(), enumerable.Count);

            logService.Verify(l => l.LogMapInUse(TestUsername));
        }

        [TestMethod]
        public async Task GetLocations()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();
            var logService = CreateMockLogService();

            var controller = new MapController(locationService.Object, imeiService.Object, logService.Object);

            var httpContext = CreateMockHttpContext();
            var context = new ControllerContext { HttpContext = httpContext.Object };

            controller.ControllerContext = context;

            var res = await controller.GetLocations();
            Assert.IsNotNull(res);

            var data = res.Data as IEnumerable<LocationViewModel>;
            Assert.IsNotNull(data);

            var locationViewModels = data as IList<LocationViewModel> ?? data.ToList();
            foreach (var d in locationViewModels)
            {
                var original = Locations.First(l => l.Id == d.Id);
                Assert.AreEqual(original.Callsign, d.Callsign);
                Assert.AreEqual(original.ReadingTime, d.ReadingTime);
                Assert.AreEqual(original.Latitude, d.Latitude);
                Assert.AreEqual(original.Longitude, d.Longitude);
                Assert.AreEqual(original.Type, d.Type);
            }

            Assert.AreEqual(Locations.Count(), locationViewModels.Count());

            logService.Verify(l => l.LogMapInUse(TestUsername));
        }

        [TestMethod]
        public void Index()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = controller.Index();

            var view = res as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task RemoveLandmark()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.ClearLandmark(TestId);

            var view = res as HttpStatusCodeResult;

            Assert.IsNotNull(view);
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)view.StatusCode);
        }

        private Mock<IIMEIService> CreateMockIMEIService()
        {
            var result = new Mock<IIMEIService>(MockBehavior.Strict);

            result.Setup(l => l.GetFromIMEI(It.Is<string>(s => s == TestIMEI)))
                .ReturnsAsync(new IMEIToCallsign
                {
                    CallSign = TestCallsign,
                    IMEI = TestIMEI
                });

            return result;
        }

        private Mock<ILocationService> CreateMockLocationService()
        {
            var result = new Mock<ILocationService>(MockBehavior.Strict);

            result.Setup(l => l.GetLocations()).ReturnsAsync(Locations);
            result.Setup(l => l.GetLandmarks()).ReturnsAsync(Landmarks);
            result.Setup(LocationRegisterExpression).Returns(Task.FromResult<object>(null));
            result.Setup(l => l.RegisterLandmark(It.Is<string>(s => s == TestLandmark), It.Is<decimal>(s => s == TestLatitude), It.Is<decimal>(s => s == TestLongitude), null))
                .Returns(Task.FromResult<object>(null));
            result.Setup(l => l.ClearLandmark(It.Is<int>(i => i == TestId)))
                .Returns(Task.FromResult<object>(null));
            result.Setup(l => l.RegisterBadLocation(TestIMEI, FailureReason.BadDateOrTime, It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<object>(null));
            result.Setup(l => l.RegisterBadLocation(string.Empty, FailureReason.NoIMEI, It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<object>(null));
            result.Setup(l => l.RegisterBadLocation(null, FailureReason.NoIMEI, It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<object>(null));
            result.Setup(l => l.RegisterBadLocation(TestIMEI, FailureReason.NoDateOrTime, It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<object>(null));
            result.Setup(l => l.RegisterBadLocation(TestIMEI, FailureReason.NoLocation, It.IsAny<DateTimeOffset>()))
                .Returns(Task.FromResult<object>(null));
            return result;
        }

        private Mock<ILogService> CreateMockLogService()
        {
            var result = new Mock<ILogService>(MockBehavior.Strict);

            result.Setup(l => l.LogMapInUse(TestUsername)).Returns(Task.FromResult<object>(null));

            return result;
        }
    }
}