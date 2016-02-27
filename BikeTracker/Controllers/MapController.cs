using BikeTracker.Controllers.Filters;
using BikeTracker.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    [AuthorizePasswordExpires]
    public class MapController : Controller
    {
        private ILocationService locationService;
        private IIMEIService imeiService;

        public MapController() : this(null, null) { }

        public MapController(ILocationService locationService, IIMEIService imeiService)
        {
            this.locationService = locationService ?? new LocationService();
            this.imeiService = imeiService ?? new IMEIService();
        }

        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetLocations()
        {
            var reportedIMEIs = await locationService.GetLocations();

            return Json(reportedIMEIs.Select(r => new
            {
                r.Id,
                r.Callsign,
                r.ReadingTime,
                r.Latitude,
                r.Longitude,
                r.Type
            }), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetLandmarks()
        {
            var registeredLandmarks = await locationService.GetLandmarks();

            return Json(registeredLandmarks, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> CheckIn(string imei, decimal? lat, decimal? lon, string time, string date)
        {
            if (lat == null || lon == null)
                return Content("No Location Given");

            if (time == null || date == null)
                return Content("No Date or Time Given");

            if (imei == null)
                return Content("No IMEI Given");

            var readingTime = DateTimeOffset.ParseExact(string.Format("{0} {1}", date, time), "ddMMyy HHmmss.fff", CultureInfo.CurrentCulture);

            await locationService.RegisterLocation(imei, readingTime, DateTimeOffset.Now, lat.Value, lon.Value);
            var callsign = await imeiService.GetFromIMEI(imei);

            return Content($"Location Receieved from {callsign.CallSign}.");
        }

        public async Task<ActionResult> AddLandmark(string name, decimal lat, decimal lon)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "name is missing");

            await locationService.RegisterLandmark(name, lat, lon);

            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        public async Task<ActionResult> ClearLandmark(int id)
        {
            await locationService.ClearLandmark(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}