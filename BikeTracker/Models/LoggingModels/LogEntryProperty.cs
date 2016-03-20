namespace BikeTracker.Models.LoggingModels
{
    public class LogEntryProperty
    {
        public virtual int Id { get; set; }
        public virtual int LogEntryId { get; set; }
        public virtual LogEntry LogEntry { get; set; }
        public virtual string PropertyType { get; set; }
        public virtual string PropertyValue { get; set; }
    }
}