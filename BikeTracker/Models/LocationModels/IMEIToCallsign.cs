using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.LocationModels
{
    /// <summary>
    /// Represents a relationship between an IMEI code and a callsign
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IMEIToCallsign
    {
        /// <summary>
        /// Gets or sets the call sign.
        /// </summary>
        /// <value>
        /// The call sign.
        /// </value>
        [Display(Name = "Call Sign"), RegularExpression(@"^[A-Z]{2}((0[1-9])|[1-9][0-9]{1,2})|WR\?\?\?$"), Required]
        public string CallSign { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the relationship.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the IMEI.
        /// </summary>
        /// <value>
        /// The IMEI.
        /// </value>
        [Required]
        public string IMEI { get; set; }

        /// <summary>
        /// Gets or sets the type of vehicle assocated with the callsign.
        /// </summary>
        /// <value>
        /// The vehicle type.
        /// </value>
        public VehicleType Type { get; set; }
    }
}