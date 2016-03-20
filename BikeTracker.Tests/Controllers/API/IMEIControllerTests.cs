using BikeTracker.Controllers.API;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly Fixture Fixture = new Fixture();
        private readonly List<IMEIToCallsign> TestCallsigns;
        private readonly int TestId;
        private readonly int BadId;
        private readonly IMEIToCallsign TestCallsign;
        private readonly string BadCallsign;

        public IMEIControllerTests()
        {
            TestCallsigns = new List<IMEIToCallsign>(Fixture.CreateMany<IMEIToCallsign>());
            TestId = Fixture.Create<int>();
            BadId = Fixture.Create<int>();
            TestCallsign = Fixture.Create<IMEIToCallsign>();
            BadCallsign = Fixture.Create<string>();
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

        public IMEIController CreateController()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);

            return controller;
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
        public async Task PutIMEIToCallsignGoodData()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var delta = new Delta<IMEIToCallsign>();
            delta.TrySetPropertyValue("IMEI", TestCallsign.IMEI);
            delta.TrySetPropertyValue("CallSign", TestCallsign.CallSign);
            delta.TrySetPropertyValue("Type", TestCallsign.Type);

            var res = await controller.Put(TestId, delta);

            Assert.IsInstanceOfType(res, typeof(UpdatedODataResult<IMEIToCallsign>));

            service.Verify(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, TestCallsign.Type));
        }

        [TestMethod]
        public async Task PutIMEIToCallsignNoIMEI()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var delta = new Delta<IMEIToCallsign>();
            delta.TrySetPropertyValue("CallSign", TestCallsign.CallSign);
            delta.TrySetPropertyValue("Type", TestCallsign.Type);

            var res = await controller.Put(TestId, delta);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PutIMEIToCallsignNoCallsign()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var delta = new Delta<IMEIToCallsign>();
            delta.TrySetPropertyValue("IMEI", TestCallsign.IMEI);
            delta.TrySetPropertyValue("Type", TestCallsign.Type);

            var res = await controller.Put(TestId, delta);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PutIMEIToCallsignNoType()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var delta = new Delta<IMEIToCallsign>();
            delta.TrySetPropertyValue("IMEI", TestCallsign.IMEI);
            delta.TrySetPropertyValue("CallSign", TestCallsign.CallSign);

            var res = await controller.Put(TestId, delta);

            Assert.IsInstanceOfType(res, typeof(UpdatedODataResult<IMEIToCallsign>));

            service.Verify(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, VehicleType.Unknown));
        }

        [TestMethod]
        public async Task PutIMEIToCallsignBadId()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var delta = new Delta<IMEIToCallsign>();
            delta.TrySetPropertyValue("IMEI", TestCallsign.IMEI);
            delta.TrySetPropertyValue("CallSign", TestCallsign.CallSign);
            delta.TrySetPropertyValue("Type", TestCallsign.Type);

            var res = await controller.Put(BadId, delta);

            Assert.IsInstanceOfType(res, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PutIMEIToCallsignBadCallsign()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var delta = new Delta<IMEIToCallsign>();
            delta.TrySetPropertyValue("IMEI", TestCallsign.IMEI);
            delta.TrySetPropertyValue("CallSign", BadCallsign);
            delta.TrySetPropertyValue("Type", TestCallsign.Type);

            var res = await controller.Put(TestId, delta);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PostIMEIToCallsignGoodData()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var res = await controller.Post(TestCallsign);

            Assert.IsInstanceOfType(res, typeof(CreatedODataResult<IMEIToCallsign>));

            service.Verify(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, TestCallsign.Type));
        }

        [TestMethod]
        public async Task PostIMEIToCallsignNoIMEI()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var callsign = new IMEIToCallsign
            {
                CallSign = TestCallsign.CallSign,
                IMEI = null,
                Type = TestCallsign.Type
            };

            Validate(callsign, controller);

            var res = await controller.Post(callsign);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PostIMEIToCallsignEmptyIMEI()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var callsign = new IMEIToCallsign
            {
                CallSign = TestCallsign.CallSign,
                IMEI = string.Empty,
                Type = TestCallsign.Type
            };

            Validate(callsign, controller);

            var res = await controller.Post(callsign);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PostIMEIToCallsignNoCallsign()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var callsign = new IMEIToCallsign
            {
                CallSign = null,
                IMEI = TestCallsign.IMEI,
                Type = TestCallsign.Type
            };

            Validate(callsign, controller);

            var res = await controller.Post(callsign);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PostIMEIToCallsignEmptyCallsign()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var callsign = new IMEIToCallsign
            {
                CallSign = string.Empty,
                IMEI = TestCallsign.IMEI,
                Type = TestCallsign.Type
            };

            Validate(callsign, controller);

            var res = await controller.Post(callsign);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task PostIMEIToCallsignNoType()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var callsign = new IMEIToCallsign
            {
                CallSign = TestCallsign.CallSign,
                IMEI = TestCallsign.IMEI,
            };

            var res = await controller.Post(callsign);

            Assert.IsInstanceOfType(res, typeof(CreatedODataResult<IMEIToCallsign>));

            service.Verify(i => i.RegisterCallsign(TestCallsign.IMEI, TestCallsign.CallSign, VehicleType.Unknown));
        }

        [TestMethod]
        public async Task PostIMEIToCallsignBadCallsign()
        {
            var service = CreateMockIMEIService();
            var controller = new IMEIController(service.Object);
            var config = new Mock<HttpConfiguration>();
            controller.Configuration = config.Object;

            var callsign = new IMEIToCallsign
            {
                CallSign = BadCallsign,
                IMEI = TestCallsign.IMEI,
                Type = TestCallsign.Type
            };

            Validate(callsign, controller);

            var res = await controller.Post(callsign);

            Assert.IsInstanceOfType(res, typeof(InvalidModelStateResult));
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

        public static void Validate(object model, ApiController controller)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);

            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);

            foreach (var v in results)
            {
                if (!v.MemberNames.Any())
                {
                    controller.ModelState.AddModelError("model", v.ErrorMessage);
                }
                else
                {
                    foreach (var m in v.MemberNames)
                        controller.ModelState.AddModelError(m, v.ErrorMessage);
                }
            }
        }
    }
}
