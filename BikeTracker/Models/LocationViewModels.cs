using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models
{
    public class CreateIMEIToCallsignViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string IMEI { get; set; }
        [Required(AllowEmptyStrings = false), 
            RegularExpression("^[A-Z]{2}((0[1-9])|[1-9]{2,3})$", ErrorMessage = "That doesn't look like a valid callsign.")]
        public string CallSign { get; set; }
        public VehicleType Type { get; set; }
    }
}