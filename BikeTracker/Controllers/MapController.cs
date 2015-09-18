using BikeTracker.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        private ApplicationDbContext dataContext = new ApplicationDbContext();

        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetLocations()
        {
            var reportedIMEIs = dataContext.LocationRecords.Select(l => l.Callsign).Distinct().Select(i =>
                 dataContext.LocationRecords.Where(l => l.Callsign == i).OrderByDescending(l => l.ReadingTime).FirstOrDefault());

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
        public ActionResult CheckIn(string imei, decimal lat, decimal lon, string time, string date)
        {
            var received = DateTimeOffset.ParseExact(string.Format("{0} {1}", date, time), "ddMMyy HHmmss.fff", CultureInfo.CurrentCulture);

            var vehicle = dataContext.IMEIToCallsigns.FirstOrDefault(i => i.IMEI == imei);

            var locationData = new LocationRecord
            {
                Latitude = lat,
                Longitude = lon,
                ReadingTime = received,
                ReceiveTime = DateTimeOffset.Now,
                Callsign = vehicle?.CallSign ?? "?????",
                Type = vehicle?.Type ?? VehicleType.Unknown
            };

            dataContext.LocationRecords.Add(locationData);
            dataContext.SaveChanges();

            return Content("Location Receieved");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dataContext.Dispose();

            base.Dispose(disposing);
        }
    }
}