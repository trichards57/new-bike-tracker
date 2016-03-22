using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="IMEIController"/> class.
        /// </summary>
        /// <param name="imeiService">The IMEI service to use.</param>
        public IMEIController(IIMEIService imeiService)
        {
            this.imeiService = imeiService;
        }

        // DELETE: odata/IMEI(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            await imeiService.DeleteIMEIById(key);

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

            return Created(await imeiService.GetFromIMEI(imeiToCallsign.IMEI));
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

            return Updated(await imeiService.GetFromIMEI(imeiToCallsign.IMEI));
        }
    }
}