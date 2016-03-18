using System;

namespace BikeTracker.Models.LocationModels
{
    public class LocationRecord
    {
        public int Id { get; set; }
        public string Callsign { get; set; }
        public VehicleType Type { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
        public DateTimeOffset ReceiveTime { get; set; }
        public bool Expired { get; set; } = false;
    }
}