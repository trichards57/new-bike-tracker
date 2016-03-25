using BikeTracker.Controllers.Filters;
using System;

namespace BikeTracker.Models.LocationModels
{
    /// <summary>
    /// Represents a location record reported to the server.
    /// </summary>
    [IgnoreCoverage]
    public class LocationRecord
    {
        /// <summary>
        /// Gets or sets the source callsign.
        /// </summary>
        /// <value>
        /// The callsign.
        /// </value>
        public string Callsign { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LocationRecord"/> is expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if expired; otherwise, <c>false</c>.
        /// </value>
        public bool Expired { get; set; } = false;
        /// <summary>
        /// Gets or sets the identifier for the report.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the latitude of the report.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public decimal Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude of the report.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public decimal Longitude { get; set; }
        /// <summary>
        /// Gets or sets the time the location reading was made.
        /// </summary>
        /// <value>
        /// The reading time.
        /// </value>
        public DateTimeOffset ReadingTime { get; set; }
        /// <summary>
        /// Gets or sets the time the location reading was received by the server.
        /// </summary>
        /// <value>
        /// The receive time.
        /// </value>
        public DateTimeOffset ReceiveTime { get; set; }
        /// <summary>
        /// Gets or sets the type of vehicle the reading is from.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public VehicleType Type { get; set; }
    }
}