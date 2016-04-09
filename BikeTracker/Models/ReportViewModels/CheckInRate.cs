using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeTracker.Models.ReportViewModels
{
    public class CheckInRate
    {
        public DateTimeOffset Time { get; set; }
        public int Count { get; set; }
    }
}