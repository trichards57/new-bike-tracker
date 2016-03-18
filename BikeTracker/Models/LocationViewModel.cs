using BikeTracker.Models.LocationModels;
using System;

namespace BikeTracker.Models
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string Callsign { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public VehicleType Type { get; set; }
    }
}