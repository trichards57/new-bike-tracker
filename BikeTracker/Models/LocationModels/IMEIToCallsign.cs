using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models.LocationModels
{
    public class IMEIToCallsign
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        [Display(Name = "Call Sign")]
        public string CallSign { get; set; }
        public VehicleType Type { get; set; }
    }
}