using BikeTracker.Models;
using BikeTracker.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BikeTracker.Controllers.API
{
    [Authorize(Roles = "GeneralAdmin")]
    public class ReportController : ApiController
    {
        private IReportService reportService;

        public ReportController(IReportService service)
        {
            reportService = service;
        }

        [HttpGet, Route("api/Report/Callsigns")]
        public async Task<IHttpActionResult> Callsigns()
        {
            return Json(await reportService.GetAllCallsigns());
        }

        [HttpGet, Route("api/Report/CallsignReportDates")]
        public async Task<IHttpActionResult> CallsignReportDates()
        {
            return Json(await reportService.GetReportDates());
        }

        [HttpGet, Route("api/Report/CallsignLocations")]
        public async Task<IHttpActionResult> CallsignLocations(string callsign, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            var start = startDate ?? DateTimeOffset.MinValue;
            var stop = endDate ?? DateTimeOffset.Now;

            return Json((await reportService.GetCallsignRecord(callsign, start, stop)).Select(r => new CallsignLocationReport
            {
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                ReadingTime = r.ReadingTime
            }));
        }
    }
}
