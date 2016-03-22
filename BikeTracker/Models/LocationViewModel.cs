using BikeTracker.Controllers.Filters;
using BikeTracker.Models.LocationModels;
using System;

namespace BikeTracker.Models
{
    [IgnoreCoverage]
    public class LocationViewModel
    {
        public string Callsign { get; set; }
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
        public VehicleType Type { get; set; }
    }
}