using BikeTracker.Controllers.API;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Results;

namespace BikeTracker.Tests.Controllers.API
{
    [TestClass]
    public class IMEIControllerTests
    {
        private readonly string BadCallsign;
        private readonly int BadId;
        private readonly Fixture Fixture = new Fixture();
        private readonly IMEIToCallsign TestCallsign;
        private readonly List<IMEIToCallsign> TestCallsigns;
        private readonly int TestId;

        public IMEIControllerTests()
        {
            TestCallsigns = new List<IMEIToCallsign>(Fixture.CreateMany<IMEIToCallsign>());
            TestId = Fixture.Create<int>();
            BadId = Fixture.Create<int>();
            TestCallsign = Fixture.Create<IMEIToCallsign>();
            BadCallsign = Fixture.Create<string>();
        }

        public IMEIController CreateController()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);

            return controller;
        }

        public Mock<IIMEIService> CreateMockIMEIService()
        {
            var service = new Mock<IIMEIService>(MockBehavior.Strict);

            service.Setup(i => i.GetAllAsync()).ReturnsAsync(TestCallsigns);
            service.Setup(i => i.GetFromIdQueryable(TestId)).ReturnsAsync((new List<IMEIToCallsign> { TestCallsign }).AsQueryable());
            service.Setup(i => i.GetFromId(BadId)).ReturnsAsync(null);
            service.Setup(i => i.GetFromId(TestId)).ReturnsAsync(TestCallsign);
            service.Setup(i => i.GetFromIMEI(TestCallsign.IMEI)).ReturnsAsync(TestCallsign);
            service.Setup(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, TestCallsign.Type)).Returns(Task.FromResult<object>(null));
            service.Setup(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, VehicleType.Unknown)).Returns(Task.FromResult<object>(null));
            service.Setup(i => i.DeleteIMEIById(TestId)).Returns(Task.FromResult<object>(null));

            return service;
        }

        [TestMethod]
        public async Task DeleteCallsign()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var res = await controller.Delete(TestId);

            var result = res as StatusCodeResult;

            Assert.IsNotNull(result);

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAllIMEIs()
        {
            var controller = CreateController();

            var res = await controller.GetIMEI();

            Assert.IsTrue(TestCallsigns.SequenceEqual(res));
        }

        [TestMethod]
        public async Task GetIMEIToCallsign()
        {
            var controller = CreateController();

            var res = await controller.GetIMEIToCallsign(TestId);

            var result = res.Queryable.First();

            Assert.AreEqual(TestCallsign.CallSign, result.CallSign);
            Assert.AreEqual(TestCallsign.Id, result.Id);
            Assert.AreEqual(TestCallsign.IMEI, result.IMEI);
            Assert.AreEqual(TestCallsign.Type, result.Type);
        }

        [TestMethod]
        public async Task PostIMEIToCallsignBadCallsign()
        {
            await PostIMEIToCallsign(BadCallsign, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [TestMethod]
        public async Task PostIMEIToCallsignEmptyCallsign()
        {
            await PostIMEIToCallsign(string.Empty, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [TestMethod]
        public async Task PostIMEIToCallsignEmptyIMEI()
        {
            await PostIMEIToCallsign(TestCallsign.CallSign, string.Empty, TestCallsign.Type, ResultType.ModelError);
        }

        [TestMethod]
        public async Task PostIMEIToCallsignGoodData()
        {
            await PostIMEIToCallsign(TestCallsign.CallSign, TestCallsign.IMEI, TestCallsign.Type);
        }

        [TestMethod]
        public async Task PostIMEIToCallsignNoCallsign()
        {
            await PostIMEIToCallsign(null, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [TestMethod]
        public async Task PostIMEIToCallsignNoIMEI()
        {
            await PostIMEIToCallsign(TestCallsign.CallSign, null, TestCallsign.Type, ResultType.ModelError);
        }

        [TestMethod]
        public async Task PutIMEIToCallsignBadCallsign()
        {
            await PutIMEIToCallsign(TestId, BadCallsign, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [TestMethod]
        public async Task PutIMEIToCallsignBadId()
        {
            await PutIMEIToCallsign(BadId, TestCallsign.CallSign, TestCallsign.IMEI, TestCallsign.Type, ResultType.NotFoundError);
        }

        [TestMethod]
        public async Task PutIMEIToCallsignGoodData()
        {
            await PutIMEIToCallsign(TestId, TestCallsign.CallSign, TestCallsign.IMEI, TestCallsign.Type);
        }

        [TestMethod]
        public async Task PutIMEIToCallsignNoCallsign()
        {
            await PutIMEIToCallsign(TestId, null, TestCallsign.IMEI, TestCallsign.Type, ResultType.ModelError);
        }

        [TestMethod]
        public async Task PutIMEIToCallsignNoIMEI()
        {
            await PutIMEIToCallsign(TestId, TestCallsign.CallSign, null, TestCallsign.Type, ResultType.ModelError);
        }

        private async Task PostIMEIToCallsign(string callsign, string imei, VehicleType type, ResultType expectedResult = ResultType.Success)
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
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
                    Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
                    break;

                case ResultType.Success:
                    Assert.IsInstanceOfType(res, typeof(CreatedODataResult<IMEIToCallsign>));
                    service.Verify(i => i.RegisterCallsign(imei, callsign, type));
                    break;
            }
        }

        private async Task PutIMEIToCallsign(int id, string callsign, string imei, VehicleType? type, ResultType expectedResult = ResultType.Success)
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
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
                    Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
                    break;

                case ResultType.NotFoundError:
                    Assert.IsInstanceOfType(res, typeof(NotFoundResult));
                    break;

                case ResultType.Success:
                    Assert.IsInstanceOfType(res, typeof(UpdatedODataResult<IMEIToCallsign>));
                    service.Verify(i => i.RegisterCallsign(imei, callsign, type));
                    break;
            }
        }
    }
}