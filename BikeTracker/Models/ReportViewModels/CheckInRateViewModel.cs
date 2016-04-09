using System;

namespace BikeTracker.Models.ReportViewModels
{
    public class CheckInRateViewModel
    {
        public int Count { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}