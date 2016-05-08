using BikeTracker.Controllers.API;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using BikeTracker.Models.ReportViewModels;
using BikeTracker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace BikeTracker.Tests.Controllers.API
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReportControllerTests
    {
        private const int DateTimeOffsetTolerance = 5;
        private readonly string BadDate;
        private readonly Fixture Fixture = new Fixture();
        private readonly string TestCallsign;
        private readonly List<string> TestCallsigns;
        private readonly DateTimeOffset TestDate;
        private readonly string TestDateString;
        private readonly List<LogEntry> TestLogEntries;
        private readonly List<LocationRecord> TestReadingsSet1;
        private readonly List<LocationRecord> TestReadingsSet2;

        public ReportControllerTests()
        {
            TestCallsigns = new List<string>(Fixture.CreateMany<string>());
            TestCallsign = TestCallsigns.First();
            TestReadingsSet1 = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
            TestReadingsSet2 = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
            TestDate = Fixture.Create<DateTimeOffset>().Date;
            TestDateString = TestDate.ToString("yyyyMMdd");
            BadDate = Fixture.Create<string>();

            TestLogEntries = new List<LogEntry>(Fixture.Build<LogEntry>().With(p => p.Type, LogEventType.UserLogIn).Without(l => l.Properties).CreateMany());
            TestLogEntries.AddRange(Fixture.Build<LogEntry>().With(p => p.Type, LogEventType.UnknownEvent).Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.Date).CreateMany());
        }

        public ReportController CreateController()
        {
            var service = CreateMockReportService();

            var logService = CreateMockLogService();

            var controller = new ReportController(service.Object, logService.Object);

            return controller;
        }

        [TestMethod]
        public async Task GetCallsignLocationsBadEndDate()
        {
            await GetCallsignLocations(TestCallsign, null, BadDate, TestReadingsSet1);
        }

        [TestMethod]
        public async Task GetCallsignLocationsBadStartDate()
        {
            await GetCallsignLocations(TestCallsign, BadDate, null, TestReadingsSet1);
        }

        [TestMethod]
        public async Task GetCallsignLocationsDefaultDates()
        {
            await GetCallsignLocations(TestCallsign, null, null, TestReadingsSet1);
        }

        [TestMethod]
        public async Task GetCallsignLocationsGoodDate()
        {
            await GetCallsignLocations(TestCallsign, TestDateString, TestDateString, TestReadingsSet2);
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
        public async Task GetDaysLogEntries()
        {
            await GetLogEntries(DateTimeOffset.Now.Date);
        }

        [TestMethod]
        public async Task GetDaysLogEntriesNullDate()
        {
            await GetLogEntries(null);
        }

        private Mock<ILogService> CreateMockLogService()
        {
            var service = new Mock<ILogService>(MockBehavior.Strict);

            service.Setup(s => s.GetLogEntries(null, null, DateTimeOffset.Now.Date, DateTimeOffset.Now.Date.AddDays(1).AddSeconds(-1))).ReturnsAsync(TestLogEntries.Where(d => d.Date.Date == DateTimeOffset.Now.Date));

            return service;
        }

        private Mock<IReportService> CreateMockReportService()
        {
            var service = new Mock<IReportService>(MockBehavior.Strict);

            service.Setup(s => s.GetAllCallsigns()).ReturnsAsync(TestCallsigns);

            service.Setup(s => s.GetCallsignRecord(TestCallsign, DateTimeOffset.MinValue, It.Is<DateTimeOffset>(d => Math.Abs((d - DateTimeOffset.Now).TotalSeconds) < DateTimeOffsetTolerance)))
                .ReturnsAsync(TestReadingsSet1);

            service.Setup(s => s.GetCallsignRecord(TestCallsign,
                It.Is<DateTimeOffset>(d => Math.Abs((d - TestDate).TotalSeconds) < DateTimeOffsetTolerance),
                It.Is<DateTimeOffset>(d => Math.Abs((d - TestDate.AddDays(1)).TotalSeconds) < DateTimeOffsetTolerance)))
                .ReturnsAsync(TestReadingsSet2);

            return service;
        }

        private async Task GetCallsignLocations(string callsign, string startDate, string endDate, List<LocationRecord> readings)
        {
            var controller = CreateController();

            var result = await controller.CallsignLocations(callsign, startDate, endDate);

            var res = result as JsonResult<IEnumerable<CallsignLocationReportViewModel>>;
            Assert.IsNotNull(res);

            foreach (var r in res.Content)
            {
                var original = readings.First(l => l.ReadingTime == r.ReadingTime);

                Assert.AreEqual(original.Latitude, r.Latitude);
                Assert.AreEqual(original.Longitude, r.Longitude);
            }

            Assert.AreEqual(readings.Count, res.Content.Count());
        }

        private async Task GetLogEntries(DateTimeOffset? date)
        {
            var controller = CreateController();

            var result = await controller.LogEntries(date?.ToString("yyyyMMdd"));

            var actualDate = date ?? DateTimeOffset.Now.Date;
            var expectedResult = TestLogEntries.Where(l => l.Date == actualDate.Date).ToList();

            Assert.IsInstanceOfType(result, typeof(JsonResult<IEnumerable<LogEntryViewModel>>));

            var res = result as JsonResult<IEnumerable<LogEntryViewModel>>;
            Assert.IsNotNull(res);

            foreach (var r in res.Content)
            {
                var original = expectedResult.First(l => l.Id == r.Id);

                Assert.AreEqual(original.Date, r.Date);
                Assert.AreEqual(LogFormatter.FormatLogEntry(original), r.Message);
                Assert.AreEqual(original.SourceUser, r.User);
            }
        }
    }
}