using BikeTracker.Controllers.Filters;
using System;

namespace BikeTracker.Models.ReportViewModels
{
    [IgnoreCoverage]
    public class CallsignLocationReportViewModel
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
    }
}