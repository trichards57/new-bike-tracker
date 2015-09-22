using BikeTracker.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        private ILocationService locationService;

        public MapController() : this(null) { }

        public MapController(ILocationService locationService)
        {
            this.locationService = locationService ?? new LocationService();
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
                r.Callsign,
                r.ReadingTime,
                r.Latitude,
                r.Longitude,
                r.Type
            }), JsonRequestBehavior.AllowGet);
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

            return Content("Location Receieved");
        }
    }
}