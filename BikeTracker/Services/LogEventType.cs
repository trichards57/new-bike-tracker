namespace BikeTracker.Services
{
    public enum LogEventType
    {
        UnknownEvent = 0,
        UserLogIn,
        UserCreated,
        UserUpdated,
        UserDeleted
    }

    public enum LogPropertyType
    {
        NewUser
    }
}