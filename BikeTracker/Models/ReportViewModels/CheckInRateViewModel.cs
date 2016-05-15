using System;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.ReportViewModels
{
    [ExcludeFromCodeCoverage]
    public class CheckInRateViewModel
    {
        public int Count { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}