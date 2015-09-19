using BikeTracker.Models;
using BikeTracker.Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    [Authorize(Roles = "IMEIAdmin,GeneralAdmin")]
    public class IMEIController : Controller
    {
        private IIMEIService imeiService;

        public IMEIController() : this(null) { }

        public IMEIController(IIMEIService imeiService)
        {
            this.imeiService = imeiService ?? new IMEIService();
        }

        // GET: IMEI
        public async Task<ActionResult> Index()
        {
            return View(await imeiService.GetAllAsync());
        }

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