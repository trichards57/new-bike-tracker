using BikeTracker.Services;
using System;
using System.Collections.Generic;

namespace BikeTracker.Models.LoggingModels
{
    public class LogEntry
    {
        public virtual int Id { get; set; }
        public virtual LogEventType Type { get; set; }
        public virtual DateTimeOffset Date { get; set; }
        public virtual string SourceUser { get; set; }
        public virtual ICollection<LogEntryProperty> Properties { get; set; } = new HashSet<LogEntryProperty>();
    }
}