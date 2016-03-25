using BikeTracker.Models.LocationModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Interface for a service that manages location entries.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Asynchronously clears the landmark identified by the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier of the landmark.</param>
        Task ClearLandmark(int id);

        /// <summary>
        /// Asynchronously gets a collection of the currently registered and valid landmarks.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Landmark}"/> containing the landmarks.</returns>
        Task<IEnumerable<Landmark>> GetLandmarks();

        /// <summary>
        /// Asynchronously gets a collection of the current location of all the registered callsigns with valid entries.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{LocationRecord}"/> containing the landmarks.</returns>
        Task<IEnumerable<LocationRecord>> GetLocations();

        /// <summary>
        /// Asynchronously registers a landmark.
        /// </summary>
        /// <param name="name">The name of the landmark.</param>
        /// <param name="latitude">The latitude of the landmark.</param>
        /// <param name="longitude">The longitude of the landmark.</param>
        /// <param name="expiry">The expiry date for the landmark.</param>
        Task RegisterLandmark(string name, decimal latitude, decimal longitude, DateTimeOffset? expiry = null);

        /// <summary>
        /// Asynchronously registers the current location of a callsign.
        /// </summary>
        /// <param name="imei">The IMEI from the location report.</param>
        /// <param name="readingTime">The time the location measurement was made.</param>
        /// <param name="receivedTime">The time the location measurement was received by the server.</param>
        /// <param name="latitude">The latitude of the callsign.</param>
        /// <param name="longitude">The longitude of the callsign.</param>
        Task RegisterLocation(string imei, DateTimeOffset readingTime, DateTimeOffset receivedTime, decimal latitude, decimal longitude);
    }
}