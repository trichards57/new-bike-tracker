using BikeTracker.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.LoggingModels
{
    /// <summary>
    /// Represents an entry in the website activity log
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class LogEntry
    {
        /// <summary>
        /// Gets or sets the date the activity occured.
        /// </summary>
        /// <value>
        /// The date of the activity.
        /// </value>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the logged activity.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the properties associated with the log entry.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public ICollection<LogEntryProperty> Properties { get; set; } = new HashSet<LogEntryProperty>();

        /// <summary>
        /// Gets or sets the user who performed the action.
        /// </summary>
        /// <value>
        /// The source user.
        /// </value>
        public string SourceUser { get; set; }

        /// <summary>
        /// Gets or sets the type of the log entry.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public LogEventType Type { get; set; }
    }
}