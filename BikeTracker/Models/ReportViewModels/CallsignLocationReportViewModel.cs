using System;

namespace BikeTracker.Models.ReportViewModels
{
    public class CallsignLocationReportViewModel
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
    }
}