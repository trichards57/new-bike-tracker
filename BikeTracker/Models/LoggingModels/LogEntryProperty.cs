using BikeTracker.Services;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.LoggingModels
{
    /// <summary>
    /// Represents a data property associated with a log entry.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class LogEntryProperty
    {
        /// <summary>
        /// Gets or sets the identifier for the property.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the log entry the property belongs to.
        /// </summary>
        /// <value>
        /// The log entry.
        /// </value>
        public LogEntry LogEntry { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the log entry the property belongs to.
        /// </summary>
        /// <value>
        /// The parent log entry identifier.
        /// </value>
        public int LogEntryId { get; set; }

        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public LogPropertyType PropertyType { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The property value.
        /// </value>
        public string PropertyValue { get; set; }
    }
}