using BikeTracker.Models.LocationModels;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models
{
    /// <summary>
    /// Represents a current location report that is passed to the map users.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LocationViewModel
    {
        /// <summary>
        /// Gets or sets the callsign for the report.
        /// </summary>
        /// <value>
        /// The callsign.
        /// </value>
        public string Callsign { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the associated location report.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the latitude from the report.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public decimal Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude from the report.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public decimal Longitude { get; set; }
        /// <summary>
        /// Gets or sets the reading time from the report.
        /// </summary>
        /// <value>
        /// The reading time.
        /// </value>
        public DateTimeOffset ReadingTime { get; set; }
        /// <summary>
        /// Gets or sets the vehicle type from the report.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public VehicleType Type { get; set; }
    }
}