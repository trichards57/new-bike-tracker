using System;

namespace BikeTracker.Models.ReportViewModels
{
    public class SuccessRateViewModel
    {
        public DateTimeOffset Time { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
    }
}