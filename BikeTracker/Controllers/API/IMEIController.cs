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
        private readonly IIMEIService _imeiService;

        private readonly ILogService _logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMEIController"/> class.
        /// </summary>
        /// <param name="imeiService">The IMEI service to use.</param>
        /// <param name="logService">The log service to use.</param>
        public IMEIController(IIMEIService imeiService, ILogService logService)
        {
            _imeiService = imeiService;
            _logService = logService;
        }

        // DELETE: odata/IMEI(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var imei = await _imeiService.GetFromId(key);

            await _imeiService.DeleteIMEIById(key);

            if (imei != null)
                await _logService.LogIMEIDeleted(User.Identity.GetUserName(), imei.IMEI);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/IMEI
        [EnableQuery]
        public async Task<IQueryable<IMEIToCallsign>> GetIMEI()
        {
            return (await _imeiService.GetAllAsync()).AsQueryable();
        }

        // GET: odata/IMEI(5)
        [EnableQuery]
        public async Task<SingleResult<IMEIToCallsign>> GetIMEIToCallsign([FromODataUri] int key)
        {
            return SingleResult.Create(await _imeiService.GetFromIdQueryable(key));
        }

        // POST: odata/IMEI
        public async Task<IHttpActionResult> Post(IMEIToCallsign imeiToCallsign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _imeiService.RegisterCallsign(imeiToCallsign.IMEI, imeiToCallsign.CallSign, imeiToCallsign.Type);

            var newIMEI = await _imeiService.GetFromIMEI(imeiToCallsign.IMEI);

            await _logService.LogIMEIRegistered(User.Identity.GetUserName(), newIMEI.IMEI, newIMEI.CallSign, newIMEI.Type);

            return Created(newIMEI);
        }

        // PUT: odata/IMEI(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<IMEIToCallsign> patch)
        {
            var imeiToCallsign = await _imeiService.GetFromId(key);
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

            await _imeiService.RegisterCallsign(imeiToCallsign.IMEI, imeiToCallsign.CallSign, imeiToCallsign.Type);

            var newIMEI = await _imeiService.GetFromIMEI(imeiToCallsign.IMEI);

            await _logService.LogIMEIRegistered(User.Identity.GetUserName(), newIMEI.IMEI, newIMEI.CallSign, newIMEI.Type);

            return Updated(newIMEI);
        }
    }
}