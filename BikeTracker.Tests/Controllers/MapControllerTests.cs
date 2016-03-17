﻿using BikeTracker.Controllers;
using BikeTracker.Models;
using BikeTracker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Tests.Controllers
{
    [TestClass]
    public class MapControllerTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly IEnumerable<LocationRecord> Locations;
        private readonly IEnumerable<Landmark> Landmarks;
        private readonly DateTimeOffset TestDate;
        private readonly decimal TestLatitude;
        private readonly decimal TestLongitude;
        private readonly string TestIMEI;
        private readonly string TestCallsign;
        private readonly Expression<Func<ILocationService, Task>> LocationRegisterExpression;
        private readonly string TestLandmark;
        private readonly int TestId;

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
        }

        private Mock<ILocationService> CreateMockLocationService()
        {
            var result = new Mock<ILocationService>(MockBehavior.Strict);

            result.Setup(l => l.GetLocations()).Returns(Task.FromResult(Locations));
            result.Setup(l => l.GetLandmarks()).Returns(Task.FromResult(Landmarks));
            result.Setup(LocationRegisterExpression).Returns(Task.FromResult<object>(null));
            result.Setup(l => l.RegisterLandmark(It.Is<string>(s => s == TestLandmark), It.Is<decimal>(s => s == TestLatitude), It.Is<decimal>(s => s == TestLongitude), null))
                .Returns(Task.FromResult<object>(null));
            result.Setup(l => l.ClearLandmark(It.Is<int>(i => i == TestId)))
                .Returns(Task.FromResult<object>(null));

            return result;
        }

        private Mock<IIMEIService> CreateMockIMEIService()
        {
            var result = new Mock<IIMEIService>(MockBehavior.Strict);

            result.Setup(l => l.GetFromIMEI(It.Is<string>(s => s == TestIMEI)))
                .Returns(Task.FromResult(new IMEIToCallsign
                {
                    CallSign = TestCallsign,
                    IMEI = TestIMEI
                }));

            return result;
        }

        [TestMethod]
        public void Index()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var res = controller.Index();

            var view = res as ViewResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task GetLocations()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var res = await controller.GetLocations();
            Assert.IsNotNull(res);

            var data = res.Data as IEnumerable<LocationViewModel>;
            Assert.IsNotNull(data);

            foreach (var d in data)
            {
                var original = Locations.FirstOrDefault(l => l.Id == d.Id);
                Assert.AreEqual(original.Callsign, d.Callsign);
                Assert.AreEqual(original.ReadingTime, d.ReadingTime);
                Assert.AreEqual(original.Latitude, d.Latitude);
                Assert.AreEqual(original.Longitude, d.Longitude);
                Assert.AreEqual(original.Type, d.Type);
            }

            Assert.AreEqual(Locations.Count(), data.Count());
        }

        [TestMethod]
        public async Task GetLandmarks()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var res = await controller.GetLandmarks();
            Assert.IsNotNull(res);

            var data = res.Data as IEnumerable<Landmark>;
            Assert.IsNotNull(data);

            foreach (var d in data)
            {
                var original = Landmarks.FirstOrDefault(l => l.Id == d.Id);
                Assert.AreEqual(original.Name, d.Name);
                Assert.AreEqual(original.Latitude, d.Latitude);
                Assert.AreEqual(original.Longitude, d.Longitude);
                Assert.AreEqual(original.Expiry, d.Expiry);
            }

            Assert.AreEqual(Locations.Count(), data.Count());
        }

        [TestMethod]
        public async Task CheckInGoodData()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testTime, testDate);

            locationService.Verify(LocationRegisterExpression);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoLatitude()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

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

            var controller = new MapController(locationService.Object, imeiService.Object);

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

            var controller = new MapController(locationService.Object, imeiService.Object);

            var testDate = TestDate.ToString("ddMMyy");
            string testTime = null;

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoDate()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            string testDate = null;
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInNoImei()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(null, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInEmptyImei()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(string.Empty, TestLatitude, TestLongitude, testTime, testDate);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task CheckInBadDate()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var testDate = TestDate.ToString("ddMMyy");
            var testTime = TestDate.ToString("HHmmss.fff");

            var res = await controller.CheckIn(TestIMEI, TestLatitude, TestLongitude, testDate, testTime);

            var view = res as ContentResult;

            Assert.IsNotNull(view);
        }

        [TestMethod]
        public async Task AddLandmarkGoodData()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

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

            var controller = new MapController(locationService.Object, imeiService.Object);

            var res = await controller.AddLandmark(null, TestLatitude, TestLongitude);

            var view = res as HttpStatusCodeResult;

            Assert.IsNotNull(view);
            Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)view.StatusCode);
        }

        [TestMethod]
        public async Task AddLandmarkEmptyName()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var res = await controller.AddLandmark(string.Empty, TestLatitude, TestLongitude);

            var view = res as HttpStatusCodeResult;

            Assert.IsNotNull(view);
            Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)view.StatusCode);
        }

        [TestMethod]
        public async Task RemoveLandmark()
        {
            var locationService = CreateMockLocationService();
            var imeiService = CreateMockIMEIService();

            var controller = new MapController(locationService.Object, imeiService.Object);

            var res = await controller.ClearLandmark(TestId);

            var view = res as HttpStatusCodeResult;

            Assert.IsNotNull(view);
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)view.StatusCode);
        }

    }
}