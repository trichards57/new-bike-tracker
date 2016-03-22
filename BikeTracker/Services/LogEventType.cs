namespace BikeTracker.Services
{
    public enum LogEventType
    {
        UnknownEvent = 0,
        UserLogIn,
        UserCreated,
        UserUpdated,
        UserDeleted,
        ImeiRegistered
    }

    public enum LogPropertyType
    {
        Imei,
        PropertyChange,
        Callsign,
        VehicleType,
        Username
    }
}