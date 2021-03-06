﻿using BikeTracker.Controllers.API;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Results;
using Xunit;

namespace BikeTracker.Tests.Controllers.API
{
    [ExcludeFromCodeCoverage]
    public class IMEIControllerTests
    {
        private readonly string BadCallsign;
        private readonly int BadId;
        private readonly Fixture Fixture = new Fixture();
        private readonly IMEIToCallsign TestCallsign;
        private readonly List<IMEIToCallsign> TestCallsigns;
        private readonly int TestId;
        private readonly string TestUsername;

        public IMEIControllerTests()
        {
            TestCallsigns = new List<IMEIToCallsign>(Fixture.CreateMany<IMEIToCallsign>());
            BadId = Fixture.Create<int>();
            TestCallsign = TestCallsigns.First();
            TestId = TestCallsign.Id;
            BadCallsign = Fixture.Create<string>();
            TestUsername = Fixture.Create<string>();
        }

        [Fact]
        public async Task DeleteCallsignBadId()
        {
            await DeleteCallsign(BadId, null, false);
        }

        [Fact]
        public async Task DeleteCallsignGoodData()
        {
            await DeleteCallsign(TestCallsign.Id, TestCallsign.IMEI, true);
        }

        [Fact]
        public async Task GetAllIMEIs()
        {
            var controller = CreateController();

            var res = await controller.GetIMEI();

            Assert.True(TestCallsigns.SequenceEqual(res));
        }

        [Fact]
        public async Task GetIMEIToCallsign()
        {
            var controller = CreateController();

            var res = await controller.GetIMEIToCallsign(TestId);

            var result = res.Queryable.First();

            Assert.Equal(TestCallsign.CallSign, result.CallSign);
            Assert.Equal(TestCallsign.Id, result.Id);
            Assert.Equal(TestCallsign.IMEI, result.IMEI);
            Assert.Equal(TestCallsign.Type, result.Type);
        }

        [Fact]
        public async Task PostIMEIToCallsignBadCallsign()
        {
            await PostIMEIToCallsign(BadCallsign, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [Fact]
        public async Task PostIMEIToCallsignEmptyCallsign()
        {
            await PostIMEIToCallsign(string.Empty, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [Fact]
        public async Task PostIMEIToCallsignEmptyIMEI()
        {
            await PostIMEIToCallsign(TestCallsign.CallSign, string.Empty, TestCallsign.Type, ResultType.ModelError);
        }

        [Fact]
        public async Task PostIMEIToCallsignGoodData()
        {
            await PostIMEIToCallsign(TestCallsign.CallSign, TestCallsign.IMEI, TestCallsign.Type);
        }

        [Fact]
        public async Task PostIMEIToCallsignNoCallsign()
        {
            await PostIMEIToCallsign(null, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [Fact]
        public async Task PostIMEIToCallsignNoIMEI()
        {
            await PostIMEIToCallsign(TestCallsign.CallSign, null, TestCallsign.Type, ResultType.ModelError);
        }

        [Fact]
        public async Task PutIMEIToCallsignBadCallsign()
        {
            await PutIMEIToCallsign(TestId, BadCallsign, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [Fact]
        public async Task PutIMEIToCallsignBadId()
        {
            await PutIMEIToCallsign(BadId, TestCallsign.CallSign, TestCallsign.IMEI, TestCallsign.Type, ResultType.NotFoundError);
        }

        [Fact]
        public async Task PutIMEIToCallsignGoodData()
        {
            await PutIMEIToCallsign(TestId, TestCallsign.CallSign, TestCallsign.IMEI, TestCallsign.Type);
        }

        [Fact]
        public async Task PutIMEIToCallsignNoCallsign()
        {
            await PutIMEIToCallsign(TestId, null, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [Fact]
        public async Task PutIMEIToCallsignNoIMEI()
        {
            await PutIMEIToCallsign(TestId, TestCallsign.CallSign, null, TestCallsign.Type, ResultType.ModelError);
        }

        private IMEIController CreateController()
        {
            var service = CreateMockIMEIService();
            var logService = CreateMockLogService();
            var controller = new IMEIController(service.Object, logService.Object);

            return controller;
        }

        private Mock<IIMEIService> CreateMockIMEIService()
        {
            var service = new Mock<IIMEIService>(MockBehavior.Strict);

            service.Setup(i => i.GetAllAsync()).ReturnsAsync(TestCallsigns);
            service.Setup(i => i.GetFromIdQueryable(TestId)).ReturnsAsync((new List<IMEIToCallsign> { TestCallsign }).AsQueryable());
            service.Setup(i => i.GetFromId(BadId)).ReturnsNullTask();
            service.Setup(i => i.GetFromId(TestId)).ReturnsAsync(TestCallsign);
            service.Setup(i => i.GetFromIMEI(TestCallsign.IMEI)).ReturnsAsync(TestCallsign);
            service.Setup(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, TestCallsign.Type)).ReturnsEmptyTask();
            service.Setup(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, VehicleType.Unknown)).ReturnsEmptyTask();
            service.Setup(i => i.DeleteIMEIById(TestId)).ReturnsEmptyTask();
            service.Setup(i => i.DeleteIMEIById(BadId)).ReturnsEmptyTask();

            return service;
        }

        private Mock<ILogService> CreateMockLogService()
        {
            var service = new Mock<ILogService>(MockBehavior.Strict);

            service.Setup(l => l.LogIMEIRegistered(TestUsername, TestCallsign.IMEI, TestCallsign.CallSign, TestCallsign.Type)).ReturnsEmptyTask();
            service.Setup(l => l.LogIMEIDeleted(TestUsername, TestCallsign.IMEI)).ReturnsEmptyTask();

            return service;
        }

        private async Task DeleteCallsign(int id, string imei, bool shouldLog)
        {
            var service = CreateMockIMEIService();
            var logService = CreateMockLogService();
            var controller = new IMEIController(service.Object, logService.Object);
            var config = new Mock<HttpConfiguration>();
            var principal = MockHelpers.CreateMockPrincipal(TestUsername);

            controller.User = principal.Object;
            controller.Configuration = config.Object;

            var res = await controller.Delete(id);

            var result = res as StatusCodeResult;

            Assert.NotNull(result);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            if (shouldLog)
            {
                logService.Verify(l => l.LogIMEIDeleted(TestUsername, imei));
            }
        }

        private async Task PostIMEIToCallsign(string callsign, string imei, VehicleType type, ResultType expectedResult = ResultType.Success)
        {
            var service = CreateMockIMEIService();
            var logService = CreateMockLogService();
            var controller = new IMEIController(service.Object, logService.Object);
            var config = new Mock<HttpConfiguration>();
            var principal = MockHelpers.CreateMockPrincipal(TestUsername);

            controller.User = principal.Object;
            controller.Configuration = config.Object;

            var imeiToCallsign = new IMEIToCallsign
            {
                CallSign = callsign,
                IMEI = imei,
                Type = type
            };

            MockHelpers.Validate(imeiToCallsign, controller);

            var res = await controller.Post(imeiToCallsign);

            switch (expectedResult)
            {
                case ResultType.ModelError:
                    Assert.IsType<InvalidModelStateResult>(res);
                    logService.Verify(l => l.LogIMEIRegistered(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<VehicleType>()), Times.Never);
                    break;

                case ResultType.Success:
                    Assert.IsType<CreatedODataResult<IMEIToCallsign>>(res);
                    service.Verify(i => i.RegisterCallsign(imei, callsign, type));
                    logService.Verify(l => l.LogIMEIRegistered(TestUsername, imei, callsign, type));
                    break;
            }
        }

        private async Task PutIMEIToCallsign(int id, string callsign, string imei, VehicleType? type, ResultType expectedResult = ResultType.Success)
        {
            var logService = CreateMockLogService();
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object, logService.Object);
            var config = new Mock<HttpConfiguration>();
            var principal = MockHelpers.CreateMockPrincipal(TestUsername);

            controller.User = principal.Object;
            controller.Configuration = config.Object;

            var delta = new Delta<IMEIToCallsign>();
            if (!string.IsNullOrEmpty(imei))
                delta.TrySetPropertyValue("IMEI", imei);
            if (!string.IsNullOrEmpty(callsign))
                delta.TrySetPropertyValue("CallSign", callsign);
            if (type != null)
                delta.TrySetPropertyValue("Type", type);

            var res = await controller.Put(id, delta);

            switch (expectedResult)
            {
                case ResultType.ModelError:
                    Assert.IsType<InvalidModelStateResult>(res);
                    logService.Verify(l => l.LogIMEIRegistered(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<VehicleType>()), Times.Never);
                    break;

                case ResultType.NotFoundError:
                    Assert.IsType<NotFoundResult>(res);
                    logService.Verify(l => l.LogIMEIRegistered(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<VehicleType>()), Times.Never);
                    break;

                case ResultType.Success:
                    Assert.IsType<UpdatedODataResult<IMEIToCallsign>>(res);
                    service.Verify(i => i.RegisterCallsign(imei, callsign, type));
                    logService.Verify(l => l.LogIMEIRegistered(TestUsername, imei, callsign, type ?? VehicleType.Unknown));
                    break;
            }
        }
    }
}