namespace BikeTracker.Models.LocationModels
{
    public enum FailureReason
    {
        NoLocation,
        NoDateOrTime,
        NoIMEI,
        BadDateOrTime,
        BadVersion
    }
}