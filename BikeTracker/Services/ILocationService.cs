using BikeTracker.Models.LocationModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface ILocationService
    {
        Task RegisterLocation(string imei, DateTimeOffset readingTime, DateTimeOffset receivedTime, decimal latitude, decimal longitude);
        Task<IEnumerable<LocationRecord>> GetLocations();
        Task RegisterLandmark(string name, decimal latitude, decimal longitude, DateTimeOffset? expiry = null);
        Task ClearLandmark(int id);
        Task<IEnumerable<Landmark>> GetLandmarks();
    }
}
