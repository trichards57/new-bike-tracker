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
        /// <summary>
        /// The data context used to store the data
        /// </summary>
        private ILocationIMEIContext dataContext;

        /// <summary>
        /// The <see cref="IIMEIService"/> used to translate IMEIs into callsigns.
        /// </summary>
        private IIMEIService imeiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class.
        /// </summary>
        /// <param name="imeiService">The IMEI service this service should use.</param>
        /// <param name="context">The data context to store to.</param>
        public LocationService(IIMEIService imeiService, ILocationIMEIContext context)
        {
            this.imeiService = imeiService;
            dataContext = context;
        }

        /// <summary>
        /// Asynchronously clears the landmark identified by the provided <paramref name="id" />.
        /// </summary>
        /// <param name="id">The identifier of the landmark.</param>
        /// <returns></returns>
        public async Task ClearLandmark(int id)
        {
            var landmark = dataContext.Landmarks.FirstOrDefault(l => l.Id == id);
            if (landmark != null)
                landmark.Expiry = DateTimeOffset.Now.AddDays(-1);

            await dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously gets a collection of the currently registered and valid landmarks.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{Landmark}" /> containing the landmarks.
        /// </returns>
        public Task<IEnumerable<Landmark>> GetLandmarks()
        {
            var landmarks = dataContext.Landmarks.Where(l => l.Expiry >= DateTimeOffset.Now);

            return Task.FromResult(landmarks.AsEnumerable());
        }

        /// <summary>
        /// Asynchronously gets a collection of the current location of all the registered callsigns with valid entries.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{LocationRecord}" /> containing the landmarks.
        /// </returns>
        public Task<IEnumerable<LocationRecord>> GetLocations()
        {
            var reportedCallsigns = dataContext.LocationRecords.Select(l => l.Callsign).Distinct();
            var latestLocations = reportedCallsigns.Select(c => dataContext.LocationRecords.Where(l => l.Callsign == c && l.Expired == false)
                .OrderByDescending(l => l.ReadingTime)
                .FirstOrDefault()).Where(l => l != null);

            return Task.FromResult(latestLocations.AsEnumerable());
        }

        /// <summary>
        /// Asynchronously registers a landmark.
        /// </summary>
        /// <param name="name">The name of the landmark.</param>
        /// <param name="latitude">The latitude of the landmark.</param>
        /// <param name="longitude">The longitude of the landmark.</param>
        /// <param name="expiry">The expiry date for the landmark.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Asynchronously registers the current location of a callsign.
        /// </summary>
        /// <param name="imei">The IMEI from the location report.</param>
        /// <param name="readingTime">The time the location measurement was made.</param>
        /// <param name="receivedTime">The time the location measurement was received by the server.</param>
        /// <param name="latitude">The latitude of the callsign.</param>
        /// <param name="longitude">The longitude of the callsign.</param>
        /// <returns></returns>
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
    }
}