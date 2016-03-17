using System;

namespace BikeTracker.Models
{
    public class CallsignLocationReport
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
    }
}