using BikeTracker.Models.LoggingModels;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BikeTracker.Models.Contexts
{
    interface ILoggingContext
    {
        DbSet<LogEntry> LogEntries { get; }
        DbSet<LogEntryProperty> LogProperties { get; }
        Task<int> SaveChangesAsync();
    }
}
