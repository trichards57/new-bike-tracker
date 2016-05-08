using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Services
{
    public class LocationService : ILocationService
    {
        /// <summary>
        /// The data context used to store the data
        /// </summary>
        private readonly ILocationContext _dataContext;

        /// <summary>
        /// The <see cref="IIMEIService"/> used to translate IMEIs into callsigns.
        /// </summary>
        private IIMEIService _imeiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class.
        /// </summary>
        /// <param name="context">The data context to store to.</param>
        /// <remarks>This is used to get around the circular reference between IMEIService and LocationService</remarks>
        [InjectionConstructor]
        public LocationService(ILocationContext context) : this(context, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class.
        /// </summary>
        /// <param name="imeiService">The IMEI service this service should use.</param>
        /// <param name="context">The data context to store to.</param>
        /// <remarks>If <paramref name="imeiService"/> is null, it will be loaded from the DependencyResolver</remarks>
        public LocationService(ILocationContext context, IIMEIService imeiService)
        {
            _imeiService = imeiService;
            _dataContext = context;
        }

        /// <summary>
        /// Gets the imei service.
        /// </summary>
        /// <value>
        /// The imei service.
        /// </value>
        private IIMEIService IMEIService => _imeiService ?? (_imeiService = DependencyResolver.Current.GetService<IIMEIService>());

        /// <summary>
        /// Asynchronously clears the landmark identified by the provided <paramref name="id" />.
        /// </summary>
        /// <param name="id">The identifier of the landmark.</param>
        /// <returns></returns>
        public async Task ClearLandmark(int id)
        {
            var landmark = _dataContext.Landmarks.FirstOrDefault(l => l.Id == id);
            if (landmark != null)
            {
                landmark.Expiry = DateTimeOffset.Now.AddDays(-1);

                await _dataContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously expires any location associated with the given callsign.
        /// </summary>
        /// <param name="callsign">The callsign.</param>
        public async Task ExpireLocation(string callsign)
        {
            if (string.IsNullOrWhiteSpace(callsign))
                return;

            var oldLocations = _dataContext.LocationRecords.Where(l => l.Callsign == callsign && l.Expired == false);

            foreach (var l in oldLocations)
                l.Expired = true;

            await _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously gets a collection of the currently registered and valid landmarks.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{Landmark}" /> containing the landmarks.
        /// </returns>
        public Task<IEnumerable<Landmark>> GetLandmarks()
        {
            var landmarks = _dataContext.Landmarks.Where(l => l.Expiry >= DateTimeOffset.Now);

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
            var reportedCallsigns = _dataContext.LocationRecords.Select(l => l.Callsign).Distinct();
            var latestLocations = reportedCallsigns.Select(c => _dataContext.LocationRecords.Where(l => l.Callsign == c && l.Expired == false)
                .OrderByDescending(l => l.ReadingTime)
                .FirstOrDefault()).Where(l => l != null);

            return Task.FromResult(latestLocations.AsEnumerable());
        }

        public async Task RegisterBadLocation(string imei, FailureReason reason, DateTimeOffset receivedTime)
        {
            var callsign = Services.IMEIService.DefaultCallsign;

            if (!string.IsNullOrWhiteSpace(imei))
                callsign = (await IMEIService.GetFromIMEI(imei)).CallSign;

            var reportData = new FailedLocationRecord
            {
                Callsign = callsign,
                Reason = reason,
                ReceivedTime = receivedTime
            };

            _dataContext.FailedRecords.Add(reportData);
            await _dataContext.SaveChangesAsync();
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
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("{0} cannot be empty or only whitespace", nameof(name));

            var landmark = new Landmark
            {
                Name = name,
                Latitude = latitude,
                Longitude = longitude,
                Expiry = expiry ?? DateTimeOffset.Now.AddDays(7)
            };

            _dataContext.Landmarks.Add(landmark);
            await _dataContext.SaveChangesAsync();
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
            if (imei == null)
                throw new ArgumentNullException(nameof(imei));

            if (string.IsNullOrWhiteSpace(imei))
                throw new ArgumentException("{0} cannot be empty or only whitespace", nameof(imei));

            var vehicle = await IMEIService.GetFromIMEI(imei);

            var locationData = new LocationRecord
            {
                Latitude = latitude,
                Longitude = longitude,
                ReadingTime = readingTime,
                ReceiveTime = receivedTime,
                Callsign = vehicle.CallSign,
                Type = vehicle.Type
            };

            _dataContext.LocationRecords.Add(locationData);
            await _dataContext.SaveChangesAsync();
        }
    }
}