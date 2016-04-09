using System;

namespace BikeTracker.Models.LocationModels
{
    public class FailedLocationRecord
    {
        /// <summary>
        /// Gets or sets the source callsign.
        /// </summary>
        /// <value>
        /// The callsign.
        /// </value>
        public string Callsign { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the report.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        public FailureReason Reason { get; set; }
        public DateTimeOffset ReceivedTime { get; set; }
    }
}