using BikeTracker.Controllers;
using BikeTracker.Models;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
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
using Xunit;

namespace BikeTracker.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
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

        [Fact]
        public async Task AddLandmarkEmptyName()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.AddLandmark(string.Empty, TestLatitude, TestLongitude);

            var view = res as HttpStatusCodeResult;

            Assert.NotNull(view);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)view.StatusCode);
        }

        [Fact]
        public async Task AddLandmarkGoodData()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.AddLandmark(TestLandmark, TestLatitude, TestLongitude);

            locationService.Verify(l => l.RegisterLandmark(It.Is<string>(s => s == TestLandmark), It.Is<decimal>(s => s == TestLatitude), It.Is<decimal>(s => s == TestLongitude), null));

            var view = res as HttpStatusCodeResult;

            Assert.NotNull(view);
            Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)view.StatusCode);
        }

        [Fact]
        public async Task AddLandmarkNoName()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.AddLandmark(null, TestLatitude, TestLongitude);

            var view = res as HttpStatusCodeResult;

            Assert.NotNull(view);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)view.StatusCode);
        }

        [Fact]
        public async Task CheckInBadDate()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testDate, testTime);

            var view = res as ContentResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task CheckInEmptyIMEI()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(string.Empty, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.NotNull(view);
        }

        [Fact]
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

            Assert.NotNull(view);
        }

        [Fact]
        public async Task CheckInNoDate()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testTime, null);

            var view = res as ContentResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task CheckInNoIMEI()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(null, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task CheckInNoLatitude()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, null, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task CheckInNoLongitude()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, null, testTime, testDate);

            var view = res as ContentResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task CheckInNoTime()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var testDate = TestDate.ToString("ddMMyy");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, null, testDate);

            var view = res as ContentResult;

            Assert.NotNull(view);
        }

        public Mock<HttpContextBase> CreateMockHttpContext()
        {
            var context = new Mock<HttpContextBase>(MockBehavior.Strict);

            context.SetupGet(h => h.User.Identity).Returns(Identity);

            return context;
        }

        [Fact]
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
            Assert.NotNull(res);

            var data = res.Data as IEnumerable<Landmark>;
            Assert.NotNull(data);

            var enumerable = data as IList<Landmark> ?? data.ToList();
            foreach (var d in enumerable)
            {
                var original = Landmarks.First(l => l.Id == d.Id);
                Assert.Equal(original.Name, d.Name);
                Assert.Equal(original.Latitude, d.Latitude);
                Assert.Equal(original.Longitude, d.Longitude);
                Assert.Equal(original.Expiry, d.Expiry);
            }

            Assert.Equal(Locations.Count(), enumerable.Count);

            logService.Verify(l => l.LogMapInUse(TestUsername));
        }

        [Fact]
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
            Assert.NotNull(res);

            var data = res.Data as IEnumerable<LocationViewModel>;
            Assert.NotNull(data);

            var locationViewModels = data as IList<LocationViewModel> ?? data.ToList();
            foreach (var d in locationViewModels)
            {
                var original = Locations.First(l => l.Id == d.Id);
                Assert.Equal(original.Callsign, d.Callsign);
                Assert.Equal(original.ReadingTime, d.ReadingTime);
                Assert.Equal(original.Latitude, d.Latitude);
                Assert.Equal(original.Longitude, d.Longitude);
                Assert.Equal(original.Type, d.Type);
            }

            Assert.Equal(Locations.Count(), locationViewModels.Count());

            logService.Verify(l => l.LogMapInUse(TestUsername));
        }

        [Fact]
        public void Index()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = controller.Index();

            var view = res as ViewResult;

            Assert.NotNull(view);
        }

        [Fact]
        public async Task RemoveLandmark()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object, null);

            var res = await controller.ClearLandmark(TestId);

            var view = res as HttpStatusCodeResult;

            Assert.NotNull(view);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)view.StatusCode);
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