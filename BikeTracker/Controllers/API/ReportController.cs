using BikeTracker.Models.ReportViewModels;
using BikeTracker.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BikeTracker.Controllers.API
{
    [Authorize(Roles = "GeneralAdmin")]
    public class ReportController : ApiController
    {
        private IReportService reportService;
        private ILogService logService;

        public ReportController(IReportService service, ILogService logService)
        {
            reportService = service;
            this.logService = logService;
        }

        [HttpGet, Route("api/Report/CallsignLocations")]
        public async Task<IHttpActionResult> CallsignLocations(string callsign, string startDate = null, string endDate = null)
        {
            var start = DateTimeOffset.MinValue;
            var stop = DateTimeOffset.Now;

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                DateTimeOffset d;
                var success = DateTimeOffset.TryParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

                if (success)
                    start = d.Date;
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                DateTimeOffset d;
                var success = DateTimeOffset.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

                if (success)
                    stop = d.Date.AddSeconds(86399);
            }

            return Json((await reportService.GetCallsignRecord(callsign, start, stop)).Select(r => new CallsignLocationReportViewModel
            {
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                ReadingTime = r.ReadingTime
            }));
        }

        [HttpGet, Route("api/Report/Callsigns")]
        public async Task<IHttpActionResult> Callsigns()
        {
            return Json(await reportService.GetAllCallsigns());
        }

        [HttpGet, Route("api/Report/LogEntries")]
        public async Task<IHttpActionResult> LogEntries(string date)
        {
            DateTimeOffset day = DateTimeOffset.Now;
            DateTimeOffset d;

            var res = DateTimeOffset.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

            if (res)
                day = d;

            var entries = await logService.GetLogEntries(startDate: day.Date, endDate: day.Date.AddDays(1).AddSeconds(-1));
            return Json(entries.Select(e => new LogEntryViewModel
            {
                Date = e.Date,
                Id = e.Id,
                Message = LogFormatter.FormatLogEntry(e),
                User = e.SourceUser
            }));
        }
    }
}