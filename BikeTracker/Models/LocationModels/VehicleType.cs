using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models.LocationModels
{
    /// <summary>
    /// Represents the type of vehicle associated with a callsign.
    /// </summary>
    public enum VehicleType
    {
        /// <summary>
        /// The vehicle type is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// The vehicle is a bike.
        /// </summary>
        Bike,

        /// <summary>
        /// The callsign belongs to a foot patrol.
        /// </summary>
        [Display(Name = "Foot Patrol")]
        FootPatrol,

        /// <summary>
        /// The vehicle is an ambulance.
        /// </summary>
        Ambulance,
        /// <summary>
        /// The callsign belongs to a different type of vehicle.
        /// </summary>
        Other
    }
}