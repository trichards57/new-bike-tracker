﻿using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace BikeTracker.Tests.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
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

        [TestMethod]
        public async Task DeleteIMEIBadIMEI()
        {
            await DeleteIMEI(BadCallsign.IMEI, tryDelete: false);
        }

        [TestMethod]
        public async Task DeleteIMEIByIdBadIMEI()
        {
            await DeleteIMEIByID(BadCallsign.Id, tryDelete: false);
        }

        [TestMethod]
        public async Task DeleteIMEIByIdGoodIMEI()
        {
            await DeleteIMEIByID(GoodCallsign.Id, GoodCallsign);
        }

        [TestMethod]
        public async Task DeleteIMEIEmptyIMEI()
        {
            await DeleteIMEI(string.Empty, tryDelete: false);
        }

        [TestMethod]
        public async Task DeleteIMEIGoodIMEI()
        {
            await DeleteIMEI(GoodCallsign.IMEI, GoodCallsign);
        }

        [TestMethod]
        public async Task DeleteIMEINoIMEI()
        {
            await DeleteIMEI(null, tryDelete: false);
        }

        [TestMethod]
        public async Task GetAll()
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            var result = await service.GetAllAsync();

            Assert.IsTrue(result.OrderBy(r => r.Id).SequenceEqual(GoodCallsigns.OrderBy(r => r.Id)));
        }

        [TestMethod]
        public async Task GetFromIdBadId()
        {
            await GetFromId(BadCallsign.Id, null);
        }

        [TestMethod]
        public async Task GetFromIdGoodId()
        {
            await GetFromId(GoodCallsign.Id, GoodCallsign);
        }

        [TestMethod]
        public async Task GetFromIdQueryableBadId()
        {
            await GetFromIdQueryable(BadCallsign.Id, null);
        }

        [TestMethod]
        public async Task GetFromIdQueryableGoodId()
        {
            await GetFromIdQueryable(GoodCallsign.Id, GoodCallsign);
        }

        [TestMethod]
        public async Task GetFromIMEIBadIMEI()
        {
            var callsign = Fixture.Create<IMEIToCallsign>();
            callsign.CallSign = IMEIService.DefaultCallsign;
            callsign.Type = VehicleType.Unknown;

            await GetFromIMEI(callsign.IMEI, callsign, shouldRegister: true);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task GetFromIMEIEmptyIMEI()
        {
            await GetFromIMEI(string.Empty, shouldReturn: false);
        }

        [TestMethod]
        public async Task GetFromIMEIGoodIMEI()
        {
            await GetFromIMEI(GoodCallsign.IMEI, GoodCallsign);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task GetFromIMEINoIMEI()
        {
            await GetFromIMEI(null, shouldReturn: false);
        }

        [TestMethod]
        public async Task RegisterCallsignCreateEmptyCallsign()
        {
            var newCallsign = string.Empty;
            var newIMEI = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(newIMEI, newCallsign, type);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task RegisterCallsignCreateEmptyIMEI()
        {
            var newCallsign = Fixture.Create<string>();
            var newIMEI = string.Empty;
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(newIMEI, newCallsign, type, false);
        }

        [TestMethod]
        public async Task RegisterCallsignCreateGoodData()
        {
            var newCallsign = Fixture.Create<string>();
            var newIMEI = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(newIMEI, newCallsign, type);
        }

        [TestMethod]
        public async Task RegisterCallsignCreateNullCallsign()
        {
            var newIMEI = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(newIMEI, null, type);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterCallsignCreateNullIMEI()
        {
            var newCallsign = Fixture.Create<string>();
            var type = VehicleType.Ambulance;

            await RegisterCallsignCreate(null, newCallsign, type, false);
        }

        [TestMethod]
        public async Task RegisterCallsignCreateNullVehicle()
        {
            var newCallsign = Fixture.Create<string>();
            var newIMEI = Fixture.Create<string>();

            await RegisterCallsignCreate(newIMEI, newCallsign, null);
        }

        [TestMethod]
        public async Task RegisterCallsignSameCallsign()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = GoodCallsign.CallSign;
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, callsign, type, GoodCallsign);
        }

        [TestMethod]
        public async Task RegisterCallsignUpdateEmptyCallsign()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = string.Empty;
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, callsign, type, GoodCallsign);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task RegisterCallsignUpdateEmptyIMEI()
        {
            var imei = string.Empty;
            var callsign = Fixture.Create<string>();
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, callsign, type, GoodCallsign, false);
        }

        [TestMethod]
        public async Task RegisterCallsignUpdateGoodData()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = Fixture.Create<string>();
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, callsign, type, GoodCallsign);
        }

        [TestMethod]
        public async Task RegisterCallsignUpdateNoType()
        {
            var imei = GoodCallsign.IMEI;
            var callsign = Fixture.Create<string>();

            await RegisterCallsignUpdate(imei, callsign, null, GoodCallsign);
        }

        [TestMethod]
        public async Task RegisterCallsignUpdateNullCallsign()
        {
            var imei = GoodCallsign.IMEI;
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(imei, null, type, GoodCallsign);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterCallsignUpdateNullIMEI()
        {
            var callsign = Fixture.Create<string>();
            var type = Fixture.Create<VehicleType>();

            await RegisterCallsignUpdate(null, callsign, type, GoodCallsign, false);
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
                Assert.IsNull(res);
            }
            else
            {
                Assert.AreEqual(expectedResult.Id, res.Id);
                Assert.AreEqual(expectedResult.CallSign, res.CallSign);
                Assert.AreEqual(expectedResult.IMEI, res.IMEI);
                Assert.AreEqual(expectedResult.Type, res.Type);
            }
        }

        private async Task GetFromIdQueryable(int id, IMEIToCallsign expectedResult)
        {
            var imeis = CreateMockIMEIDbSet();
            var context = CreateMockIMEIContext(imeis.Object);
            var locationService = CreateMockLocationService();

            var service = new IMEIService(context.Object, locationService.Object);

            var res = await service.GetFromIdQueryable(id);

            Assert.IsInstanceOfType(res, typeof(IQueryable<IMEIToCallsign>));

            if (expectedResult == null)
            {
                Assert.IsFalse(res.Any());
            }
            else
            {
                Assert.AreEqual(1, res.Count());
                var actual = res.First();
                Assert.AreEqual(expectedResult.Id, actual.Id);
                Assert.AreEqual(expectedResult.CallSign, actual.CallSign);
                Assert.AreEqual(expectedResult.IMEI, actual.IMEI);
                Assert.AreEqual(expectedResult.Type, actual.Type);
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
                Assert.IsNotNull(res);
                Assert.IsNotNull(expectedResult);

                if (!shouldRegister)
                {
                    // If the system is expected to register the item, ID won't have been generated
                    Assert.AreEqual(expectedResult.Id, res.Id);
                }
                Assert.AreEqual(expectedResult.CallSign, res.CallSign);
                Assert.AreEqual(expectedResult.IMEI, res.IMEI);
                Assert.AreEqual(expectedResult.Type, res.Type);
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
                Assert.AreEqual(imei, linkedObject.IMEI);
                Assert.AreEqual(string.IsNullOrWhiteSpace(callsign) ? oldValues.CallSign : callsign, linkedObject.CallSign);
                Assert.AreEqual(type ?? oldValues.Type, linkedObject.Type);
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                Assert.AreEqual(imei, linkedObject.IMEI);
                Assert.AreEqual(oldValues.CallSign, linkedObject.CallSign);
                Assert.AreEqual(oldValues.Type, linkedObject.Type);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }
    }
}