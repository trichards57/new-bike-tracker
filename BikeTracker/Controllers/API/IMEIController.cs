using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace BikeTracker.Controllers.API
{
    /// <summary>
    /// Controller to provide the OData API for IMEI Management
    /// </summary>
    [Authorize(Roles = "IMEIAdmin,GeneralAdmin")]
    public class IMEIController : ODataController
    {
        /// <summary>
        /// The IMEI Service used to access the database
        /// </summary>
        private IIMEIService imeiService;
        private ILogService logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMEIController"/> class.
        /// </summary>
        /// <param name="imeiService">The IMEI service to use.</param>
        public IMEIController(IIMEIService imeiService, ILogService logService)
        {
            this.imeiService = imeiService;
            this.logService = logService;
        }

        // DELETE: odata/IMEI(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var imei = await imeiService.GetFromId(key);

            await imeiService.DeleteIMEIById(key);

            if (imei != null)
                await logService.LogImeiDeleted(User.Identity.GetUserName(), imei.IMEI);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/IMEI
        [EnableQuery]
        public async Task<IQueryable<IMEIToCallsign>> GetIMEI()
        {
            return (await imeiService.GetAllAsync()).AsQueryable();
        }

        // GET: odata/IMEI(5)
        [EnableQuery]
        public async Task<SingleResult<IMEIToCallsign>> GetIMEIToCallsign([FromODataUri] int key)
        {
            return SingleResult.Create(await imeiService.GetFromIdQueryable(key));
        }

        // POST: odata/IMEI
        public async Task<IHttpActionResult> Post(IMEIToCallsign imeiToCallsign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await imeiService.RegisterCallsign(imeiToCallsign.IMEI, imeiToCallsign.CallSign, imeiToCallsign.Type);

            var newImei = await imeiService.GetFromIMEI(imeiToCallsign.IMEI);

            await logService.LogImeiRegistered(User.Identity.GetUserName(), newImei.IMEI, newImei.CallSign, newImei.Type);

            return Created(newImei);
        }

        // PUT: odata/IMEI(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<IMEIToCallsign> patch)
        {
            var imeiToCallsign = await imeiService.GetFromId(key);
            if (imeiToCallsign == null)
            {
                return NotFound();
            }

            patch.Put(imeiToCallsign);

            Validate(imeiToCallsign);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await imeiService.RegisterCallsign(imeiToCallsign.IMEI, imeiToCallsign.CallSign, imeiToCallsign.Type);

            var newImei = await imeiService.GetFromIMEI(imeiToCallsign.IMEI);

            await logService.LogImeiRegistered(User.Identity.GetUserName(), newImei.IMEI, newImei.CallSign, newImei.Type);

            return Updated(newImei);
        }
    }
}