using BikeTracker.Controllers.Filters;
using System;

namespace BikeTracker.Models.ReportViewModels
{
    /// <summary>
    /// Represents a location report for a given callsign
    /// </summary>
    [IgnoreCoverage]
    public class CallsignLocationReportViewModel
    {
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
        /// Gets or sets the time the location report reading was taken.
        /// </summary>
        /// <value>
        /// The reading time.
        /// </value>
        public DateTimeOffset ReadingTime { get; set; }
    }
}