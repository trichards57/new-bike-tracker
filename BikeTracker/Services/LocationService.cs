using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public class LocationService : ILocationService
    {
        private ILocationIMEIContext dataContext;
        private IIMEIService imeiService;

        public LocationService(IIMEIService imeiService, ILocationIMEIContext dataContext)
        {
            this.imeiService = imeiService;
            this.dataContext = dataContext;
        }

        public Task<IEnumerable<LocationRecord>> GetLocations()
        {
            var reportedCallsigns = dataContext.LocationRecords.Select(l => l.Callsign).Distinct();
            var latestLocations = reportedCallsigns.Select(c => dataContext.LocationRecords.Where(l => l.Callsign == c && l.Expired == false)
                .OrderByDescending(l => l.ReadingTime)
                .FirstOrDefault()).Where(l => l != null);

            return Task.FromResult(latestLocations.AsEnumerable());
        }

        public async Task RegisterLocation(string imei, DateTimeOffset readingTime, DateTimeOffset receivedTime, decimal latitude, decimal longitude)
        {
            var vehicle = await imeiService.GetFromIMEI(imei);

            var locationData = new LocationRecord
            {
                Latitude = latitude,
                Longitude = longitude,
                ReadingTime = readingTime,
                ReceiveTime = receivedTime,
                Callsign = vehicle.CallSign,
                Type = vehicle.Type
            };

            dataContext.LocationRecords.Add(locationData);
            await dataContext.SaveChangesAsync();
        }

        public async Task RegisterLandmark(string name, decimal latitude, decimal longitude, DateTimeOffset? expiry = null)
        {
            var landmark = new Landmark
            {
                Name = name,
                Latitude = latitude,
                Longitude = longitude,
                Expiry = expiry ?? DateTimeOffset.Now.AddDays(7)
            };

            dataContext.Landmarks.Add(landmark);
            await dataContext.SaveChangesAsync();
        }

        public Task<IEnumerable<Landmark>> GetLandmarks()
        {
            var landmarks = dataContext.Landmarks.Where(l => l.Expiry >= DateTimeOffset.Now);

            return Task.FromResult(landmarks.AsEnumerable());
        }

        public async Task ClearLandmark(int id)
        {
            var landmark = dataContext.Landmarks.FirstOrDefault(l => l.Id == id);
            if (landmark != null)
                landmark.Expiry = DateTimeOffset.Now.AddDays(-1);

            await dataContext.SaveChangesAsync();
        }
    }
}