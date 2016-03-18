using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models.LocationModels
{
    public enum VehicleType
    {
        Unknown,
        Bike,
        [Display(Name = "Foot Patrol")]
        FootPatrol,
        Ambulance,
        Other
    }
}