using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace BikeTracker.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class LocationServiceTests
    {
        private const int DateTimeTolerance = 5;
        private readonly Landmark BadLandmark;
        private readonly Fixture Fixture = new Fixture();
        private readonly IMEIToCallsign GoodCallsign;
        private readonly Landmark GoodLandmark;
        private readonly List<Landmark> GoodLandmarks;
        private readonly LocationRecord GoodLocation;
        private readonly List<LocationRecord> GoodLocations;
        private readonly string UnknownIMEI;

        public LocationServiceTests()
        {
            GoodLocations = new List<LocationRecord>(Fixture.CreateMany<LocationRecord>());
            GoodLocation = GoodLocations.First();
            BadLandmark = Fixture.Create<Landmark>();
            GoodCallsign = Fixture.Create<IMEIToCallsign>();
            UnknownIMEI = Fixture.Create<string>();
            GoodLandmarks = new List<Landmark>(Fixture.CreateMany<Landmark>());
            GoodLandmark = GoodLandmarks.First();
        }

        [Fact]
        public async Task ClearLandmarkBadId()
        {
            await ClearLandmark(BadLandmark.Id);
        }

        [Fact]
        public async Task ClearLandmarkGoodData()
        {
            await ClearLandmark(GoodLandmark.Id, GoodLandmark);
        }

        [Fact]
        public async Task ExpireLocationEmptyCallsign()
        {
            await ExpireLocation(string.Empty, false);
        }

        [Fact]
        public async Task ExpireLocationGoodCallsign()
        {
            await ExpireLocation(GoodCallsign.CallSign, true);
        }

        [Fact]
        public async Task ExpireLocationNullCallsign()
        {
            await ExpireLocation(null, false);
        }

        [Fact]
        public async Task GetLandmarks()
        {
            var lm = Fixture.Create<Landmark>();
            lm.Expiry = DateTimeOffset.Now.AddDays(-1);
            GoodLandmarks.Add(lm);

            lm = Fixture.Create<Landmark>();
            lm.Expiry = DateTimeOffset.Now.AddDays(1);
            GoodLandmarks.Add(lm);

            var landmarks = CreateMockLandmarkDbSet();
            var context = CreateMockLocationContext(landmarks: landmarks.Object);

            var service = new LocationService(context.Object);

            var res = await service.GetLandmarks();

            Assert.True(GoodLandmarks.Where(l => (l.Expiry - DateTimeOffset.Now).TotalSeconds > 5).OrderBy(l => l.Id).SequenceEqual(res.OrderBy(l => l.Id)));
        }

        [Fact]
        public async Task GetLocations()
        {
            GoodLocations.Clear();

            GoodLocations.AddRange(new[]
            {
                new LocationRecord
                {
                     Callsign = "WR01",
                     ReadingTime = new DateTimeOffset(2016, 1, 1, 1, 1, 1, TimeSpan.Zero),
                     Latitude = 1,
                     Longitude = 1
                },
                new LocationRecord
                {
                     Callsign = "WR01",
                     ReadingTime = new DateTimeOffset(2016, 1, 1, 1, 2, 1, TimeSpan.Zero),
                     Latitude = 2,
                     Longitude = 2
                },
                new LocationRecord
                {
                     Callsign = "WR02",
                     ReadingTime = new DateTimeOffset(2016, 1, 1, 3, 1, 1, TimeSpan.Zero),
                     Latitude = 3,
                     Longitude = 3
                },
                new LocationRecord
                {
                     Callsign = "WR02",
                     ReadingTime = new DateTimeOffset(2016, 1, 1, 1, 1, 1, TimeSpan.Zero),
                     Latitude = 4,
                     Longitude = 4
                },
                new LocationRecord
                {
                     Callsign = "WR03",
                     ReadingTime = new DateTimeOffset(2016, 1, 1, 1, 1, 1, TimeSpan.Zero),
                     Latitude = 5,
                     Longitude = 5
                }
            });

            var locations = MockHelpers.CreateMockLocationDbSet(GoodLocations);
            var context = CreateMockLocationContext(locations.Object);

            var service = new LocationService(context.Object);

            var res = (await service.GetLocations()).ToList();

            var wr01 = res.Single(l => l.Callsign == "WR01");
            Assert.Equal(2, wr01.Latitude);
            Assert.Equal(2, wr01.Longitude);

            var wr02 = res.Single(l => l.Callsign == "WR02");
            Assert.Equal(3, wr02.Latitude);
            Assert.Equal(3, wr02.Longitude);

            var wr03 = res.Single(l => l.Callsign == "WR03");
            Assert.Equal(5, wr03.Latitude);
            Assert.Equal(5, wr03.Longitude);
        }

        [Fact]
        public async Task RegisterLandmarkEmptyName()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => RegisterLandmark(string.Empty, GoodLandmark.Latitude, GoodLandmark.Longitude, GoodLandmark.Expiry, false));
        }

        [Fact]
        public async Task RegisterLandmarkGoodData()
        {
            await RegisterLandmark(GoodLandmark.Name, GoodLandmark.Latitude, GoodLandmark.Longitude, GoodLandmark.Expiry, true);
        }

        [Fact]
        public async Task RegisterLandmarkNullExpiry()
        {
            await RegisterLandmark(GoodLandmark.Name, GoodLandmark.Latitude, GoodLandmark.Longitude, null, true);
        }

        [Fact]
        public async Task RegisterLandmarkNullName()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => RegisterLandmark(null, GoodLandmark.Latitude, GoodLandmark.Longitude, GoodLandmark.Expiry, false));
        }

        [Fact]
        public async Task RegisterLocationEmptyIMEI()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => RegisterLocation(new IMEIToCallsign { IMEI = string.Empty }, GoodLocation.ReadingTime, GoodLocation.ReceiveTime, GoodLocation.Latitude, GoodLocation.Longitude));
        }

        [Fact]
        public async Task RegisterLocationGoodData()
        {
            await RegisterLocation(GoodCallsign, GoodLocation.ReadingTime, GoodLocation.ReceiveTime, GoodLocation.Latitude, GoodLocation.Longitude);
        }

        [Fact]
        public async Task RegisterLocationNullIMEI()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => RegisterLocation(new IMEIToCallsign { IMEI = null }, GoodLocation.ReadingTime, GoodLocation.ReceiveTime, GoodLocation.Latitude, GoodLocation.Longitude));
        }

        [Fact]
        public async Task RegisterLocationUnknownIMEI()
        {
            await RegisterLocation(new IMEIToCallsign { IMEI = UnknownIMEI, CallSign = IMEIService.DefaultCallsign }, GoodLocation.ReadingTime, GoodLocation.ReceiveTime, GoodLocation.Latitude, GoodLocation.Longitude, shouldUseResolver: true);
        }

        private static Mock<ILocationContext> CreateMockLocationContext(DbSet<LocationRecord> locations = null, DbSet<Landmark> landmarks = null)
        {
            var context = new Mock<ILocationContext>(MockBehavior.Strict);

            context.SetupGet(c => c.LocationRecords).Returns(locations);
            context.SetupGet(c => c.Landmarks).Returns(landmarks);
            context.Setup(s => s.SaveChangesAsync()).Returns(Task.FromResult(1));

            return context;
        }

        private static bool ValidateLandmarkRecord(Landmark landmark, string name, decimal latitude, decimal longitude, DateTimeOffset? expiry)
        {
            if (expiry == null)
                expiry = DateTimeOffset.Now.AddDays(7);

            Assert.Equal(latitude, landmark.Latitude);
            Assert.Equal(longitude, landmark.Longitude);
            Assert.Equal(name, landmark.Name);
            Assert.True(Math.Abs((expiry.Value - landmark.Expiry).TotalSeconds) < DateTimeTolerance);
            return true;
        }

        private static bool ValidateLocationRecord(LocationRecord record, IMEIToCallsign imei, decimal latitude, decimal longitude, DateTimeOffset readingTime, DateTimeOffset receivedTime)
        {
            Assert.Equal(latitude, record.Latitude);
            Assert.Equal(longitude, record.Longitude);
            Assert.Equal(readingTime, record.ReadingTime);
            Assert.Equal(receivedTime, record.ReceiveTime);
            Assert.Equal(imei.CallSign, record.Callsign);
            Assert.Equal(imei.Type, record.Type);

            return true;
        }

        private async Task ClearLandmark(int id, Landmark linkedObject = null)
        {
            var landmarks = CreateMockLandmarkDbSet();
            var context = CreateMockLocationContext(landmarks: landmarks.Object);

            var service = new LocationService(context.Object);

            await service.ClearLandmark(id);

            if (linkedObject != null)
            {
                Assert.True((linkedObject.Expiry - DateTimeOffset.Now).TotalSeconds < -DateTimeTolerance);
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private Mock<IIMEIService> CreateMockIMEIService()
        {
            var service = new Mock<IIMEIService>(MockBehavior.Strict);

            service.Setup(s => s.GetFromIMEI(GoodCallsign.IMEI)).ReturnsAsync(GoodCallsign);
            service.Setup(s => s.GetFromIMEI(UnknownIMEI)).ReturnsAsync(new IMEIToCallsign { CallSign = IMEIService.DefaultCallsign, IMEI = UnknownIMEI });

            return service;
        }

        private Mock<DbSet<Landmark>> CreateMockLandmarkDbSet()
        {
            var mockLocationEntrySet = new Mock<DbSet<Landmark>>();

            var data = GoodLandmarks.AsQueryable();

            mockLocationEntrySet.Setup(e => e.Add(It.IsAny<Landmark>())).Callback<Landmark>(i => GoodLandmarks.Add(i));

            mockLocationEntrySet.As<IDbAsyncEnumerable<Landmark>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Landmark>(data.GetEnumerator()));

            mockLocationEntrySet.As<IQueryable<Landmark>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Landmark>(data.Provider));

            mockLocationEntrySet.As<IQueryable<Landmark>>().Setup(m => m.Expression).Returns(data.Expression);
            mockLocationEntrySet.As<IQueryable<Landmark>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockLocationEntrySet.As<IQueryable<Landmark>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockLocationEntrySet;
        }

        private async Task ExpireLocation(string callsign, bool shouldUpdate)
        {
            if (shouldUpdate)
            {
                var lr = Fixture.Create<LocationRecord>();
                lr.Callsign = callsign;
                lr.Expired = false;
                GoodLocations.Add(lr);
            }

            var locations = MockHelpers.CreateMockLocationDbSet(GoodLocations);
            var context = CreateMockLocationContext(locations.Object);

            var service = new LocationService(context.Object);

            await service.ExpireLocation(callsign);

            if (shouldUpdate)
            {
                foreach (var gl in GoodLocations.Where(l => l.Callsign == callsign))
                    Assert.True(gl.Expired);

                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task RegisterLandmark(string name, decimal latitude, decimal longitude, DateTimeOffset? expiry, bool shouldStore)
        {
            var landmarks = CreateMockLandmarkDbSet();
            var context = CreateMockLocationContext(landmarks: landmarks.Object);

            var service = new LocationService(context.Object);

            await service.RegisterLandmark(name, latitude, longitude, expiry);

            if (shouldStore)
            {
                landmarks.Verify(l => l.Add(It.Is<Landmark>(lr => ValidateLandmarkRecord(lr, name, latitude, longitude, expiry))));
                context.Verify(c => c.SaveChangesAsync());
            }
        }

        private async Task RegisterLocation(IMEIToCallsign imei, DateTimeOffset readingTime, DateTimeOffset receivedTime, decimal latitude, decimal longitude, bool shouldStore = true, bool shouldUseResolver = false)
        {
            var locations = MockHelpers.CreateMockLocationDbSet(GoodLocations);
            var context = CreateMockLocationContext(locations.Object);
            var imeiService = CreateMockIMEIService();
            var container = new UnityContainer();

            container.RegisterInstance(imeiService.Object);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            var service = new LocationService(context.Object, shouldUseResolver ? null : imeiService.Object);

            await service.RegisterLocation(imei.IMEI, readingTime, receivedTime, latitude, longitude);

            if (shouldStore)
            {
                locations.Verify(l => l.Add(It.Is<LocationRecord>(lr => ValidateLocationRecord(lr, imei, latitude, longitude, readingTime, receivedTime))));
                context.Verify(c => c.SaveChangesAsync());
            }
        }
    }
}