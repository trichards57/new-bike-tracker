using BikeTracker.Controllers.API;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.ReportViewModels;
using BikeTracker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace BikeTracker.Tests.Controllers.API
{
    [TestClass]
    public class ReportControllerTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly List<string> TestCallsigns;
        private readonly List<DateTimeOffset> TestDates;
        private readonly DateTimeOffset TestDate;
        private readonly string TestDateString;
        private readonly string TestCallsign;
        private readonly List<LocationRecord> TestReadingsSet1;
        private readonly List<LocationRecord> TestReadingsSet2;
        private readonly List<LocationRecord> TestReadingsSet3;
        private readonly List<LocationRecord> TestReadingsSet4;
        private const int DateTimeOffsetTolerance = 5; // seconds
        private readonly string BadDate;

        public ReportControllerTests()
        {
            TestCallsigns = new List<string>(Fixture.CreateMany<string>());
            TestDates = new List<DateTimeOffset>(Fixture.CreateMany<DateTimeOffset>());
            TestCallsign = TestCallsigns.First();
            TestReadingsSet1 = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
            TestReadingsSet2 = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
            TestReadingsSet3 = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
            TestReadingsSet4 = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
            TestDate = TestDates.First().Date;
            TestDateString = TestDate.ToString("yyyyMMdd");
            BadDate = Fixture.Create<string>();
        }

        public Mock<IReportService> CreateMockReportService()
        {
            var service = new Mock<IReportService>(MockBehavior.Strict);

            service.Setup(s => s.GetAllCallsigns()).ReturnsAsync(TestCallsigns);
            service.Setup(s => s.GetReportDates()).ReturnsAsync(TestDates);

            service.Setup(s => s.GetCallsignRecord(TestCallsign, DateTimeOffset.MinValue, It.Is<DateTimeOffset>(d => Math.Abs((d - DateTimeOffset.Now).TotalSeconds) < DateTimeOffsetTolerance)))
                .ReturnsAsync(TestReadingsSet1);

            service.Setup(s => s.GetCallsignRecord(TestCallsign,
                It.Is<DateTimeOffset>(d => Math.Abs((d - TestDate).TotalSeconds) < DateTimeOffsetTolerance)
                , It.Is<DateTimeOffset>(d => Math.Abs((d - TestDate.AddDays(1)).TotalSeconds) < DateTimeOffsetTolerance)))
                .ReturnsAsync(TestReadingsSet2);

            return service;
        }

        public ReportController CreateController()
        {
            var service = CreateMockReportService();
            var controller = new ReportController(service.Object);

            return controller;
        }

        [TestMethod]
        public async Task GetCallsigns()
        {
            var controller = CreateController();

            var result = await controller.Callsigns();

            var res = result as JsonResult<IEnumerable<string>>;
            Assert.IsNotNull(res);

            Assert.IsTrue(TestCallsigns.SequenceEqual(res.Content));
        }

        [TestMethod]
        public async Task GetCallsignReportDates()
        {
            var controller = CreateController();

            var result = await controller.CallsignReportDates();

            var res = result as JsonResult<IEnumerable<DateTimeOffset>>;
            Assert.IsNotNull(res);

            Assert.IsTrue(TestDates.SequenceEqual(res.Content));
        }

        [TestMethod]
        public async Task GeCallsignLocationsDefaultDates()
        {
            var controller = CreateController();

            var result = await controller.CallsignLocations(TestCallsign);

            var res = result as JsonResult<IEnumerable<CallsignLocationReportViewModel>>;
            Assert.IsNotNull(res);

            foreach (var r in res.Content)
            {
                var original = TestReadingsSet1.First(l => l.ReadingTime == r.ReadingTime);

                Assert.AreEqual(original.Latitude, r.Latitude);
                Assert.AreEqual(original.Longitude, r.Longitude);
            }

            Assert.AreEqual(TestReadingsSet1.Count, res.Content.Count());
        }

        [TestMethod]
        public async Task GeCallsignLocationsBadStartDate()
        {
            var controller = CreateController();

            var result = await controller.CallsignLocations(TestCallsign, BadDate);

            var res = result as JsonResult<IEnumerable<CallsignLocationReportViewModel>>;
            Assert.IsNotNull(res);

            foreach (var r in res.Content)
            {
                var original = TestReadingsSet1.First(l => l.ReadingTime == r.ReadingTime);

                Assert.AreEqual(original.Latitude, r.Latitude);
                Assert.AreEqual(original.Longitude, r.Longitude);
            }

            Assert.AreEqual(TestReadingsSet1.Count, res.Content.Count());
        }

        [TestMethod]
        public async Task GeCallsignLocationsBadEndDate()
        {
            var controller = CreateController();

            var result = await controller.CallsignLocations(TestCallsign, endDate:BadDate);

            var res = result as JsonResult<IEnumerable<CallsignLocationReportViewModel>>;
            Assert.IsNotNull(res);

            foreach (var r in res.Content)
            {
                var original = TestReadingsSet1.First(l => l.ReadingTime == r.ReadingTime);

                Assert.AreEqual(original.Latitude, r.Latitude);
                Assert.AreEqual(original.Longitude, r.Longitude);
            }

            Assert.AreEqual(TestReadingsSet1.Count, res.Content.Count());
        }

        [TestMethod]
        public async Task GetCallsignLocationsGoodDate()
        {
            var controller = CreateController();

            var result = await controller.CallsignLocations(TestCallsign, TestDateString, TestDateString);

            var res = result as JsonResult<IEnumerable<CallsignLocationReportViewModel>>;
            Assert.IsNotNull(res);

            foreach (var r in res.Content)
            {
                var original = TestReadingsSet2.First(l => l.ReadingTime == r.ReadingTime);

                Assert.AreEqual(original.Latitude, r.Latitude);
                Assert.AreEqual(original.Longitude, r.Longitude);
            }

            Assert.AreEqual(TestReadingsSet2.Count, res.Content.Count());
        }
    }
}
