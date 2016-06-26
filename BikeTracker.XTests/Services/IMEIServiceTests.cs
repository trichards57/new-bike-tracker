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
    public class IMEIServiceTests
    {
        private readonly IMEIToCallsign BadCallsign;
        private readonly Fixture Fixture = new Fixture();
        private readonly IMEIToCallsign GoodCallsign;
        private readonly List<IMEIToCallsign> GoodCallsigns;

        public IMEIServiceTests()
        {
            GoodCallsigns = new List<IMEIToCallsign>(Fixture.CreateMany<IMEIToCallsign>());
            GoodCallsign = GoodCallsigns.First();
            BadCallsign = Fixture.Create<IMEIToCallsign>();
        }

        public Mock<IIMEIContext> CreateMockIMEIContext(DbSet<IMEIToCallsign> callsigns)
        {
            var context = new Mock<IIMEIContext>(MockBehavior.Strict);

            context.SetupGet(c => c.IMEIToCallsigns).Returns(callsigns);
            context.Setup(s => s.SaveChangesAsync()).Returns(Task.FromResult(1));

            return context;
        }

        public Mock<DbSet<IMEIToCallsign>> CreateMockIMEIDbSet()
        {
            var mockIMEIEntrySet = new Mock<DbSet<IMEIToCallsign>>();

            var data = GoodCallsigns.AsQueryable();

            mockIMEIEntrySet.Setup(e => e.Add(It.IsAny<IMEIToCallsign>())).Callback<IMEIToCallsign>(i => GoodCallsigns.Add(i));

            mockIMEIEntrySet.As<IDbAsyncEnumerable<IMEIToCallsign>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<IMEIToCallsign>(data.GetEnumerator()));

            mockIMEIEntrySet.As<IQueryable<IMEIToCallsign>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<IMEIToCallsign>(data.Provider));

            mockIMEIEntrySet.As<IQueryable<IMEIToCallsign>>().Setup(m => m.Expression).Returns(data.Expression);
            mockIMEIEntrySet.As<IQueryable<IMEIToCallsign>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockIMEIEntrySet.As<IQueryable<IMEIToCallsign>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockIMEIEntrySet;
        }

        public Mock<ILocationService> CreateMockLocationService()
        {
            var service = new Mock<ILocationService>(MockBehavior.Strict);

            service.Setup(s => s.ExpireLocation(GoodCallsign.CallSign)).Returns(Task.FromResult<object>(null));

            return service;
        }

        [Fact]
        public async Task DeleteIMEIBadIMEI()
        {
            await DeleteIMEI(BadCallsign.IMEI, tryDelete: false);
        }

        [Fact]
        public async Task DeleteIMEIByIdBadIMEI()
        {
            await DeleteIMEIByID(BadCallsign.Id, tryDelete: false);
        }

        [Fact]
        public async Task DeleteIMEIByIdGoodIMEI()
        {
            await DeleteIMEIByID(GoodCallsign.Id, GoodCallsign);
        }

        [Fact]
        public async Task DeleteIMEIEmptyIMEI()
        {
            await DeleteIMEI(string.Empty, tryDelete: false);
        }

        [Fact]
        public async Task DeleteIMEIGoodIMEI()
        {
            await DeleteIMEI(GoodCallsign.IMEI, GoodCallsign);
        }

        [Fact]
        public async Task DeleteIMEINoIMEI()
        {
            await DeleteIMEI(null, tryDelete: false);
        }

        [Fact]
        public async Task GetAll()
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            var result = await service.GetAllAsync();

            Assert.True(result.OrderBy(r => r.Id).SequenceEqual(GoodCallsigns.OrderBy(r => r.Id)));
        }

        [Fact]
        public async Task GetFromIdBadId()
        {
            await GetFromId(BadCallsign.Id, null);
        }

        [Fact]
        public async Task GetFromIdGoodId()
        {
            await GetFromId(GoodCallsign.Id, GoodCallsign);
        }

        [Fact]
        public async Task GetFromIdQueryableBadId()
        {
            await GetFromIdQueryable(BadCallsign.Id, null);
        }

        [Fact]
        public async Task GetFromIdQueryableGoodId()
        {
            await GetFromIdQueryable(GoodCallsign.Id, GoodCallsign);
        }

        [Fact]
        public async Task GetFromIMEIBadIMEI()
        {
            var callsign = Fixture.Create<IMEIToCallsign>();
            callsign.CallSign = IMEIService.DefaultCallsign;
            callsign.Type = VehicleType.Unknown;

            await GetFromIMEI(callsign.IMEI, callsign, shouldRegister: true);
        }

        [Fact]
        public async Task GetFromIMEIEmptyIMEI()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => GetFromIMEI(string.Empty, shouldReturn: false));
        }

        [Fact]
        public async Task GetFromIMEIGoodIMEI()
        {
            await GetFromIMEI(GoodCallsign.IMEI, GoodCallsign);
        }

        [Fact]
        public async Task GetFromIMEINoIMEI()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => GetFromIMEI(null, shouldReturn: false));
        }

        [Fact]
        public async Task RegisterCallsignCreateEmptyCallsign()
        {
            var newCallsign = string.Empty;
            var newIMEI = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(newIMEI, newCallsign, type);
        }

        [Fact]
        public async Task RegisterCallsignCreateEmptyIMEI()
        {
            var newCallsign = Fixture.Create<string>();
            var newIMEI = string.Empty;
            var type = VehicleType.Ambulance;

            await Assert.ThrowsAsync<ArgumentException>(() => RegisterCallsignCreate(newIMEI, newCallsign, type, false));
        }

        [Fact]
        public async Task RegisterCallsignCreateGoodData()
        {
            var newCallsign = Fixture.Create<string>();
            var newIMEI = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(newIMEI, newCallsign, type);
        }

        [Fact]
        public async Task RegisterCallsignCreateNullCallsign()
        {
            var newIMEI = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(newIMEI, null, type);
        }

        [Fact]
        public async Task RegisterCallsignCreateNullIMEI()
        {
            var newCallsign = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await Assert.ThrowsAsync<ArgumentNullException>(() => RegisterCallsignCreate(null, newCallsign, type, false));
        }

        [Fact]
        public async Task RegisterCallsignCreateNullVehicle()
        {
            var newCallsign = Fixture.Create<string>();
            var newIMEI = Fixture.Create<string>();

            await RegisterCallsignCreate(newIMEI, newCallsign, null);
        }

        [Fact]
        public async Task RegisterCallsignSameCallsign()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = GoodCallsign.CallSign;
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, callsign, type, GoodCallsign);
        }

        [Fact]
        public async Task RegisterCallsignUpdateEmptyCallsign()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = string.Empty;
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, callsign, type, GoodCallsign);
        }

        [Fact]
        public async Task RegisterCallsignUpdateEmptyIMEI()
        {
            var imei = string.Empty;
            var callsign = Fixture.Create<string>();
            var type = Fixture.Create<VehicleType>();

            await Assert.ThrowsAsync<ArgumentException>(() => RegisterCallsignUpdate(imei, callsign, type, GoodCallsign, false));
        }

        [Fact]
        public async Task RegisterCallsignUpdateGoodData()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = Fixture.Create<string>();
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, callsign, type, GoodCallsign);
        }

        [Fact]
        public async Task RegisterCallsignUpdateNoType()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = Fixture.Create<string>();

            await RegisterCallsignUpdate(imei, callsign, null, GoodCallsign);
        }

        [Fact]
        public async Task RegisterCallsignUpdateNullCallsign()
        {
            var imei = GoodCallsign.IMEI;
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, null, type, GoodCallsign);
        }

        [Fact]
        public async Task RegisterCallsignUpdateNullIMEI()
        {
            var callsign = Fixture.Create<string>();
            var type = Fixture.Create<VehicleType>();

            await Assert.ThrowsAsync<ArgumentNullException>(() => RegisterCallsignUpdate(null, callsign, type, GoodCallsign, false));
        }

        private static void ConfirmDelete(Mock<DbSet<IMEIToCallsign>> imeis, Mock<IIMEIContext> context, Mock<ILocationService> locationService, IMEIToCallsign imeiObj, bool tryDelete)
        {
            if (tryDelete)
            {
                imeis.Verify(i => i.Remove(imeiObj));
                locationService.Verify(s => s.ExpireLocation(imeiObj.CallSign));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                imeis.Verify(i => i.Remove(It.IsAny<IMEIToCallsign>()), Times.Never);
                locationService.Verify(s => s.ExpireLocation(It.IsAny<string>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task DeleteIMEI(string imei, IMEIToCallsign imeiObj = null, bool tryDelete = true)
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var container = new UnityContainer();

            container.RegisterInstance(locationService.Object);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            var service = new IMEIService(context.Object);

            await service.DeleteIMEI(imei);

            ConfirmDelete(imeis, context, locationService, imeiObj, tryDelete);
        }

        private async Task DeleteIMEIByID(int id, IMEIToCallsign imeiObj = null, bool tryDelete = true)
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            await service.DeleteIMEIById(id);
            ConfirmDelete(imeis, context, locationService, imeiObj, tryDelete);
        }

        private async Task GetFromId(int id, IMEIToCallsign expectedResult)
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            var res = await service.GetFromId(id);

            if (expectedResult == null)
            {
                Assert.Null(res);
            }
            else
            {
                Assert.Equal(expectedResult.Id, res.Id);
                Assert.Equal(expectedResult.CallSign, res.CallSign);
                Assert.Equal(expectedResult.IMEI, res.IMEI);
                Assert.Equal(expectedResult.Type, res.Type);
            }
        }

        private async Task GetFromIdQueryable(int id, IMEIToCallsign expectedResult)
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            var res = await service.GetFromIdQueryable(id);

            Assert.IsType<IQueryable<IMEIToCallsign>>(res);

            if (expectedResult == null)
            {
                Assert.False(res.Any());
            }
            else
            {
                Assert.Equal(1, res.Count());
                var actual = res.First();
                Assert.Equal(expectedResult.Id, actual.Id);
                Assert.Equal(expectedResult.CallSign, actual.CallSign);
                Assert.Equal(expectedResult.IMEI, actual.IMEI);
                Assert.Equal(expectedResult.Type, actual.Type);
            }
        }

        private async Task GetFromIMEI(string imei, IMEIToCallsign expectedResult = null, bool shouldReturn = true, bool shouldRegister = false)
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            var res = await service.GetFromIMEI(imei);

            if (shouldRegister)
            {
                imeis.Verify(i => i.Add(It.Is<IMEIToCallsign>(c => c.IMEI == imei && c.CallSign == IMEIService.DefaultCallsign && c.Type == VehicleType.Unknown)));
                locationService.Verify(s => s.ExpireLocation(IMEIService.DefaultCallsign), Times.Never);
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                imeis.Verify(i => i.Add(It.IsAny<IMEIToCallsign>()), Times.Never);
                locationService.Verify(s => s.ExpireLocation(It.IsAny<string>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }

            if (shouldReturn)
            {
                Assert.NotNull(res);
                Assert.NotNull(expectedResult);

                if (!shouldRegister)
                {
                    // If the system is expected to register the item, ID won't have been generated
                    Assert.Equal(expectedResult.Id, res.Id);
                }
                Assert.Equal(expectedResult.CallSign, res.CallSign);
                Assert.Equal(expectedResult.IMEI, res.IMEI);
                Assert.Equal(expectedResult.Type, res.Type);
            }
        }

        private async Task RegisterCallsignCreate(string imei, string callsign, VehicleType? type, bool shouldCreate = true)
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            await service.RegisterCallsign(imei, callsign, type);

            if (shouldCreate)
            {
                var expectedItem = new IMEIToCallsign
                {
                    IMEI = imei,
                    CallSign = callsign ?? IMEIService.DefaultCallsign,
                    Type = type ?? VehicleType.Unknown
                };

                imeis.Verify(i => i.Add(It.Is<IMEIToCallsign>(c => c.IMEI == expectedItem.IMEI && c.CallSign == expectedItem.CallSign && c.Type == expectedItem.Type)));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                imeis.Verify(i => i.Add(It.IsAny<IMEIToCallsign>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task RegisterCallsignUpdate(string imei, string callsign, VehicleType? type, IMEIToCallsign linkedObject, bool shouldUpdate = true)
        {
            var oldValues = new IMEIToCallsign
            {
                CallSign = linkedObject.CallSign,
                Id = linkedObject.Id,
                IMEI = linkedObject.IMEI,
                Type = linkedObject.Type
            };

            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            await service.RegisterCallsign(imei, callsign, type);

            if (string.IsNullOrWhiteSpace(callsign))
            {
                locationService.Verify(l => l.ExpireLocation(It.IsAny<string>()), Times.Never);
            }
            else
            {
                locationService.Verify(l => l.ExpireLocation(oldValues.CallSign));
            }

            if (shouldUpdate)
            {
                Assert.Equal(imei, linkedObject.IMEI);
                Assert.Equal(string.IsNullOrWhiteSpace(callsign) ? oldValues.CallSign : callsign, linkedObject.CallSign);
                Assert.Equal(type ?? oldValues.Type, linkedObject.Type);
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                Assert.Equal(imei, linkedObject.IMEI);
                Assert.Equal(oldValues.CallSign, linkedObject.CallSign);
                Assert.Equal(oldValues.Type, linkedObject.Type);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }
    }
}