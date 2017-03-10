using BikeTracker.Models.ReportViewModels;
using BikeTracker.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BikeTracker.Controllers.API
{
    [Authorize(Roles = "GeneralAdmin")]
    public class ReportController : ApiController
    {
        private readonly ILogService _logService;
        private readonly IReportService _reportService;

        public ReportController(IReportService service, ILogService logService)
        {
            _reportService = service;
            _logService = logService;
        }

        [HttpGet, Route("api/Report/CallsignLocations")]
        public async Task<IHttpActionResult> CallsignLocations(string callsign, string startDate = null, string endDate = null)
        {
            var start = DateTimeOffset.MinValue;
            var stop = DateTimeOffset.Now;

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                var success = DateTimeOffset.TryParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d);

                if (success)
                    start = d.Date;
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                var success = DateTimeOffset.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d);

                if (success)
                    stop = d.Date.AddSeconds(86399);
            }

            return Json((await _reportService.GetCallsignRecord(callsign, start, stop)).Select(r => new CallsignLocationReportViewModel
            {
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                ReadingTime = r.ReadingTime
            }));
        }

        [HttpGet, Route("api/Report/Callsigns")]
        public async Task<IHttpActionResult> Callsigns()
        {
            return Json(await _reportService.GetAllCallsigns());
        }

        [HttpGet, Route("api/Report/CheckInRatesByHour")]
        public async Task<IHttpActionResult> CheckInRatesByHour(string callsign, string date)
        {
            var start = DateTimeOffset.Now;

            if (!string.IsNullOrWhiteSpace(date))
            {
                var success = DateTimeOffset.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d);

                if (success)
                    start = d.Date;
            }

            var data = await _reportService.GetCheckInRatesByHour(callsign, start);

            return Json(data);
        }

        [HttpGet, Route("api/Report/DownloadCallsignLocations")]
        public async Task<IHttpActionResult> DownloadCallsignLocations(string callsign, string startDate = null, string endDate = null)
        {
            var start = TryParseDate(startDate, DateTimeOffset.MinValue).Date;
            var stop = TryParseDate(endDate, DateTimeOffset.Now).Date.AddSeconds(86399);

            var data = await _reportService.GetCallsignRecord(callsign, start, stop);

            var outputFile = new StringBuilder();

            outputFile.AppendLine("Reading Time, Latitude, Longitude");

            foreach (var d in data)
                outputFile.AppendLine($"{d.ReadingTime.ToString("u")}, {d.Latitude}, {d.Longitude}");

            return new FileStringResult { Content = outputFile.ToString(), Filename = $"{callsign}.csv" };
        }

        [HttpGet, Route("api/Report/LogEntries")]
        public async Task<IHttpActionResult> LogEntries(string date)
        {
            var day = DateTimeOffset.Now;

            var res = DateTimeOffset.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d);

            if (res)
                day = d;

            var entries = await _logService.GetLogEntries(startDate: day.Date, endDate: day.Date.AddDays(1).AddSeconds(-1));
            return Json(entries.Select(e => new LogEntryViewModel
            {
                Date = e.Date,
                Id = e.Id,
                Message = LogFormatter.FormatLogEntry(e),
                User = e.SourceUser
            }));
        }

        [HttpGet, Route("api/Report/SuccessRatesByHour")]
        public async Task<IHttpActionResult> SuccessRatesByHour(string callsign, string date)
        {
            var start = DateTimeOffset.Now;

            if (!string.IsNullOrWhiteSpace(date))
            {
                var success = DateTimeOffset.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d);

                if (success)
                    start = d.Date;
            }

            var data = await _reportService.GetSuccessRatesByHour(callsign, start);

            return Json(data);
        }

        private static DateTimeOffset TryParseDate(string date, DateTimeOffset defaultDate)
        {
            if (string.IsNullOrWhiteSpace(date)) return defaultDate;

            var success = DateTimeOffset.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d);

            return success ? d : defaultDate;
        }
    }
}