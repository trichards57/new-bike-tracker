using System;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.ReportViewModels
{
    [ExcludeFromCodeCoverage]
    public class SuccessRateViewModel
    {
        public int FailureCount { get; set; }
        public int SuccessCount { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}