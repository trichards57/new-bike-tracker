using System;

namespace BikeTracker.Models.LocationModels
{
    public class Landmark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTimeOffset Expiry { get; set; }
    }
}