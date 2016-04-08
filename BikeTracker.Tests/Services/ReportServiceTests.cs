using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Tests.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReportServiceTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly List<LocationRecord> GoodLocations;

        public ReportServiceTests()
        {
            GoodLocations = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
        }

        public Mock<ILocationContext> CreateMockLocationContext(DbSet<LocationRecord> locations = null, DbSet<Landmark> landmarks = null)
        {
            var context = new Mock<ILocationContext>(MockBehavior.Strict);

            context.SetupGet(c => c.LocationRecords).Returns(locations);

            return context;
        }

        [TestMethod]
        public async Task GetAllCallsigns()
        {
            var locations = MockHelpers.CreateMockLocationDbSet(GoodLocations);
            var context = CreateMockLocationContext(locations.Object);

            var service = new ReportService(context.Object);

            var res = await service.GetAllCallsigns();

            Assert.IsTrue(GoodLocations.Select(l => l.Callsign).Distinct().OrderBy(s => s).SequenceEqual(res.OrderBy(s => s)));
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task GetCallsignRecordEmptyCallsign()
        {
            await GetCallsignRecord(string.Empty);
        }

        [TestMethod]
        public async Task GetCallsignRecordGoodData()
        {
            GoodLocations.Clear();

            var badRecord = new LocationRecord
            {
                Callsign = "WR01",
                ReadingTime = DateTimeOffset.Now,
                Latitude = 1
            };
            var badRecord2 = new LocationRecord
            {
                Callsign = "WR02",
                ReadingTime = DateTimeOffset.Now.AddDays(-1),
                Latitude = 1
            };

            GoodLocations.Add(badRecord);
            GoodLocations.Add(badRecord2);

            var goodRecords = new[]
            {
                new LocationRecord
                {
                    Callsign = "WR02",
                    ReadingTime = DateTimeOffset.Now.AddMinutes(-1),
                    Latitude = 1
                },
                new LocationRecord
                {
                    Callsign = "WR02",
                    ReadingTime = DateTimeOffset.Now.AddMinutes(-2),
                    Latitude = 1
                },
                new LocationRecord
                {
                    Callsign = "WR02",
                    ReadingTime = DateTimeOffset.Now.AddMinutes(-3),
                    Latitude = 1
                },
                new LocationRecord
                {
                    Callsign = "WR02",
                    ReadingTime = DateTimeOffset.Now.AddMinutes(-4),
                    Latitude = 1
                },
            };

            GoodLocations.AddRange(goodRecords);

            var locations = MockHelpers.CreateMockLocationDbSet(GoodLocations);
            var context = CreateMockLocationContext(locations.Object);

            var service = new ReportService(context.Object);

            var res = await service.GetCallsignRecord("WR02", DateTimeOffset.Now.AddHours(-1), DateTimeOffset.Now);

            Assert.IsTrue(res.OrderBy(l => l.ReadingTime).SequenceEqual(goodRecords.OrderBy(l => l.ReadingTime)));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task GetCallsignRecordNullCallsign()
        {
            await GetCallsignRecord(null);
        }

        private async Task GetCallsignRecord(string callsign)
        {
            var locations = MockHelpers.CreateMockLocationDbSet(GoodLocations);
            var context = CreateMockLocationContext(locations.Object);

            var service = new ReportService(context.Object);

            await service.GetCallsignRecord(callsign, DateTimeOffset.Now.AddHours(-1), DateTimeOffset.Now);
        }
    }
}