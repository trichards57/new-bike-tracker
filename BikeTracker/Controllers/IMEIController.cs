using BikeTracker.Models;
using BikeTracker.Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// Controller that deals with all of the IMEI to Callsign activities
    /// </summary>
    [Authorize(Roles = "IMEIAdmin,GeneralAdmin")]
    public class IMEIController : Controller
    {
        /// <summary>
        /// The IMEI Service used to access the database
        /// </summary>
        private IIMEIService imeiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMEIController"/> class.
        /// </summary>
        public IMEIController() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IMEIController"/> class.
        /// </summary>
        /// <param name="imeiService">The IMEI service to use.</param>
        /// <remarks>
        /// This overload is used to allow dependency injection for testing.
        /// </remarks>
        public IMEIController(IIMEIService imeiService)
        {
            this.imeiService = imeiService ?? new IMEIService();
        }

        /// <summary>
        /// Displays the IMEI Admin control panel
        /// </summary>
        /// <returns>The Index view</returns>
        /// @mapping GET /IMEI/
        /// @mapping GET /IMEI/Index
        /// @notanon
        /// @role{GeneralAdmin}
        /// @role{IMEIAdmin}
        public async Task<ActionResult> Index()
        {
            return View(await imeiService.GetAllAsync());
        }

        /// <summary>
        /// Displays the details associated with an IMEI to Callsign record
        /// </summary>
        /// <param name="id">The id of the IMEI to Callsign record.</param>
        /// <returns>
        /// A Bad Request result if <paramref name="id"/> is not provided.
        /// A File Not Found if no record is linked with the id.
        /// Otherwise the Details view.
        /// </returns>
        /// @mapping GET /IMEI/Details
        /// @notanon
        /// @role{GeneralAdmin}
        /// @role{IMEIAdmin}
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var imeiToCallsign = await imeiService.GetFromId(id.Value);

            if (imeiToCallsign == null)
                return HttpNotFound();

            return View(imeiToCallsign);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateIMEIToCallsignViewModel imeiToCallsign)
        {
            if (ModelState.IsValid)
            {
                await imeiService.RegisterCallsign(imeiToCallsign.IMEI, imeiToCallsign.CallSign, imeiToCallsign.Type);
                return RedirectToAction("Index");
            }

            return View(imeiToCallsign);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var imeiToCallsign = await imeiService.GetFromId(id.Value);
            if (imeiToCallsign == null)
                return HttpNotFound();

            var model = new CreateIMEIToCallsignViewModel
            {
                CallSign = imeiToCallsign.CallSign,
                IMEI = imeiToCallsign.IMEI,
                Type = imeiToCallsign.Type
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateIMEIToCallsignViewModel imeiToCallsign)
        {
            if (ModelState.IsValid)
            {
                await imeiService.RegisterCallsign(imeiToCallsign.IMEI, imeiToCallsign.CallSign, imeiToCallsign.Type);
                return RedirectToAction("Index");
            }

            return View(imeiToCallsign);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var iMEIToCallsign = await imeiService.GetFromId(id.Value);
            if (iMEIToCallsign == null)
            {
                return HttpNotFound();
            }
            return View(iMEIToCallsign);
        }

        // POST: LocationTracker/IMEIToCallsigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await imeiService.DeleteIMEIById(id);
            return RedirectToAction("Index");
        }
    }
}