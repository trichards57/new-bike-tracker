using BikeTracker.Models.LoggingModels;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BikeTracker.Models.Contexts
{
    /// <summary>
    /// An interface for a data context that can store log entries.
    /// </summary>
    public interface ILoggingContext
    {
        /// <summary>
        /// Gets the log entries.
        /// </summary>
        /// <value>
        /// The log entries.
        /// </value>
        DbSet<LogEntry> LogEntries { get; }
        /// <summary>
        /// Gets the log properties.
        /// </summary>
        /// <value>
        /// The log properties.
        /// </value>
        DbSet<LogEntryProperty> LogProperties { get; }

        /// <summary>
        /// Asynchronously saves the changes to the data context.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}