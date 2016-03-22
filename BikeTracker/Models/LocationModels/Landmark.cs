using BikeTracker.Controllers.Filters;
using System;

namespace BikeTracker.Models.LocationModels
{
    [IgnoreCoverage]
    public class Landmark
    {
        public DateTimeOffset Expiry { get; set; }
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Name { get; set; }
    }
}