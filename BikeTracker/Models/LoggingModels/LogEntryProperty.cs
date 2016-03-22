using BikeTracker.Controllers.Filters;
using BikeTracker.Services;

namespace BikeTracker.Models.LoggingModels
{
    [IgnoreCoverage]
    public class LogEntryProperty
    {
        public virtual int Id { get; set; }
        public virtual LogEntry LogEntry { get; set; }
        public virtual int LogEntryId { get; set; }
        public virtual LogPropertyType PropertyType { get; set; }
        public virtual string PropertyValue { get; set; }
    }
}