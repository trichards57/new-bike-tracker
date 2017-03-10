using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BikeTracker.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class ReportServiceTests
    {
        private readonly Fixture Fixture = new Fixture();
        private readonly List<LocationRecord> GoodLocations;

        public ReportServiceTests()
        {
            GoodLocations = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
        }

        public Mock<ILocationContext> CreateMockLocationContext(DbSet<LocationRecord> locations = null)
        {
            var context = new Mock<ILocationContext>(MockBehavior.Strict);

            context.SetupGet(c => c.LocationRecords).Returns(locations);

            return context;
        }

        [Fact]
        public async Task GetAllCallsigns()
        {
            var locations = MockHelpers.CreateMockLocationDbSet(GoodLocations);
            var context = CreateMockLocationContext(locations.Object);

            var service = new ReportService(context.Object);

            var res = await service.GetAllCallsigns();

            Assert.True(GoodLocations.Select(l => l.Callsign).Distinct().OrderBy(s => s).SequenceEqual(res.OrderBy(s => s)));
        }

        [Fact]
        public async Task GetCallsignRecordEmptyCallsign()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => GetCallsignRecord(string.Empty));
        }

        [Fact]
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

            Assert.True(res.OrderBy(l => l.ReadingTime).SequenceEqual(goodRecords.OrderBy(l => l.ReadingTime)));
        }

        [Fact]
        public async Task GetCallsignRecordNullCallsign()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => GetCallsignRecord(null));
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