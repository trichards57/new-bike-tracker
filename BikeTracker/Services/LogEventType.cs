namespace BikeTracker.Services
{
    /// <summary>
    /// The event type the log entry is reporting.
    /// </summary>
    public enum LogEventType
    {
        /// <summary>
        /// Log event unknown
        /// </summary>
        UnknownEvent = 0,
        /// <summary>
        /// A user logged in
        /// </summary>
        UserLogIn,
        /// <summary>
        /// A user was created
        /// </summary>
        UserCreated,
        /// <summary>
        /// A user's details were updated
        /// </summary>
        UserUpdated,
        /// <summary>
        /// A user was deleted
        /// </summary>
        UserDeleted,
        /// <summary>
        /// An IMEI was registered
        /// </summary>
        ImeiRegistered,
        ImeiDeleted
    }

    /// <summary>
    /// The data type associated with the log entry.
    /// </summary>
    public enum LogPropertyType
    {
        /// <summary>
        /// The IMEI associated with a log entry
        /// </summary>
        Imei,
        /// <summary>
        /// A property that was changed
        /// </summary>
        PropertyChange,
        /// <summary>
        /// The callsign associated with a log entry
        /// </summary>
        Callsign,
        /// <summary>
        /// The vehicle type associated with a log entry
        /// </summary>
        VehicleType,
        /// <summary>
        /// The username associated with a log entry
        /// </summary>
        Username
    }
}