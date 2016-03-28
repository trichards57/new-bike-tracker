using System;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.LocationModels
{
    /// <summary>
    /// Represents a landmark registered with the server.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Landmark
    {
        /// <summary>
        /// Gets or sets the expiry date of the landmark.
        /// </summary>
        /// <value>
        /// The expiry.
        /// </value>
        public DateTimeOffset Expiry { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the landmark.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the latitude of the landmark.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public decimal Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude of the landmark.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public decimal Longitude { get; set; }
        /// <summary>
        /// Gets or sets the name of the landmark.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}