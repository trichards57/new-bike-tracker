using BikeTracker.Controllers.Filters;
using BikeTracker.Models;
using BikeTracker.Models.LocationModels;
using BikeTracker.Services;
using Microsoft.AspNet.Identity;
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
        private IIMEIService imeiService;
        private ILocationService locationService;
        private ILogService logService;

        public MapController(ILocationService locationService, IIMEIService imeiService, ILogService logService)
        {
            this.locationService = locationService;
            this.imeiService = imeiService;
            this.logService = logService;
        }

        public async Task<ActionResult> AddLandmark(string name, decimal lat, decimal lon)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "name is missing");

            await locationService.RegisterLandmark(name, lat, lon);

            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [AllowAnonymous]
        public async Task<ActionResult> CheckIn(string imei, decimal? lat, decimal? lon, string time, string date, int v = 1)
        {
            int version = v;
            DateTimeOffset readingTime;

            if (lat == null || lon == null)
            {
                await locationService.RegisterBadLocation(imei, FailureReason.NoLocation, DateTimeOffset.Now);
                return Content("No Location Given");
            }

            if (version == 1)
            {
                if (time == null || date == null)
                {
                    await locationService.RegisterBadLocation(imei, FailureReason.NoDateOrTime, DateTimeOffset.Now);
                    return Content("No Date or Time Given");
                }

                var result = DateTimeOffset.TryParseExact(string.Format("{0} {1}", date, time), "ddMMyy HHmmss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out readingTime);

                if (!result)
                {
                    await locationService.RegisterBadLocation(imei, FailureReason.BadDateOrTime, DateTimeOffset.Now);
                    return Content("Bad Date or Time Given");
                }

            }
            else if (version == 2)
            {
                if (time == null)
                {
                    await locationService.RegisterBadLocation(imei, FailureReason.NoDateOrTime, DateTimeOffset.Now);
                    return Content("No Date or Time Given");
                }

                var result = DateTimeOffset.TryParse(time, out readingTime);

                if (!result)
                {
                    await locationService.RegisterBadLocation(imei, FailureReason.BadDateOrTime, DateTimeOffset.Now);
                    return Content("Bad Date or Time Given");
                }
            }
            else
            {
                await locationService.RegisterBadLocation(imei, FailureReason.BadVersion, DateTimeOffset.Now);
                return Content("Bad Version Given");
            }

            if (string.IsNullOrEmpty(imei))
            {
                await locationService.RegisterBadLocation(imei, FailureReason.NoIMEI, DateTimeOffset.Now);
                return Content("No IMEI Given");
            }

            await locationService.RegisterLocation(imei, readingTime, DateTimeOffset.Now, lat.Value, lon.Value);
            var callsign = await imeiService.GetFromIMEI(imei);

            return Content($"Location Receieved from {callsign.CallSign}.");
        }

        public async Task<ActionResult> ClearLandmark(int id)
        {
            await locationService.ClearLandmark(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<JsonResult> GetLandmarks()
        {
            var registeredLandmarks = await locationService.GetLandmarks();

            await logService.LogMapInUse(User.Identity.GetUserName());

            return Json(registeredLandmarks, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetLocations()
        {
            var reportedIMEIs = await locationService.GetLocations();

            await logService.LogMapInUse(User.Identity.GetUserName());

            return Json(reportedIMEIs.Select(r => new LocationViewModel
            {
                Id = r.Id,
                Callsign = r.Callsign,
                ReadingTime = r.ReadingTime,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Type = r.Type
            }), JsonRequestBehavior.AllowGet);
        }

        // GET: Map
        public ActionResult Index()
        {
            return View();
        }
    }
}