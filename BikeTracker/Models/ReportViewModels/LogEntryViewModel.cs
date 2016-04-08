using System;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.ReportViewModels
{
    [ExcludeFromCodeCoverage]
    public class LogEntryViewModel
    {
        public DateTimeOffset Date { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
    }
}