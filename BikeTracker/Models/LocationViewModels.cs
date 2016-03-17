using System;
using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models
{
    public class CreateIMEIToCallsignViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string IMEI { get; set; }
        [Required(AllowEmptyStrings = false), 
            RegularExpression("^[A-Z]{2}((0[1-9])|[1-9][0-9]{1,2})$", ErrorMessage = "That doesn't look like a valid callsign.")]
        public string CallSign { get; set; }
        public VehicleType Type { get; set; }
    }

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