using BikeTracker.Controllers.Filters;
using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models.LocationModels
{
    [IgnoreCoverage]
    public class IMEIToCallsign
    {
        [Display(Name = "Call Sign"), RegularExpression("^[A-Z]{2}((0[1-9])|[1-9][0-9]{1,2})$"), Required]
        public string CallSign { get; set; }

        public int Id { get; set; }

        [Required]
        public string IMEI { get; set; }

        public VehicleType Type { get; set; }
    }
}