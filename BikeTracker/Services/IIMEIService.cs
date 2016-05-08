using BikeTracker.Models.LocationModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Interface for a service that allows the management of IMEI to callsign relationships.
    /// </summary>
    public interface IIMEIService
    {
        /// <summary>
        /// Asynchronously deletes the IMEI associated with the provided <paramref name="id"/> from the service's records.
        /// </summary>
        /// <param name="id">The identifier for the IMEI entry.</param>
        /// This function will not report an error if the IMEI does not exist.
        Task DeleteIMEIById(int id);

        /// <summary>
        /// Asynchronously gets all of the IMEI to callsign relationships.
        /// </summary>
        /// <returns>
        /// An IEnumerable&lt;IMEIToCallsign&gt; containing all of the currently registered relationships.
        /// </returns>
        Task<IEnumerable<IMEIToCallsign>> GetAllAsync();

        /// <summary>
        /// Asynchronously gets the IMEI to callsign relationship identified by the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier for the IMEI entry.</param>
        /// <returns>
        /// If a relationship under the provided ID exists, it is returned in an IMEIToCallsign.
        /// Otherwise returns null.
        /// </returns>
        Task<IMEIToCallsign> GetFromId(int id);

        /// <summary>
        /// Asynchronously gets the IMEI to callsign relationship identified by the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier for the IMEI entry.</param>
        /// <returns>
        /// If a relationship under the provided ID exists, it is returned in an IMEIToCallsign contained in an IQueryable.
        /// Otherwise returns an empty IQueryable.
        /// </returns>
        Task<IQueryable<IMEIToCallsign>> GetFromIdQueryable(int id);

        /// <summary>
        /// Asynchronously gets the IMEI to callsign relationship containing the provided <paramref name="imei"/>.
        /// </summary>
        /// <param name="imei">The IMEI to search for.</param>
        /// <returns>
        /// Returns the relationship in an IMEIToCallsign.
        /// </returns>
        /// <remarks>
        /// If the relationship does not exist when called, it will be created.  It will be given an unknown callsign
        /// and the vehicle type will be set to <see cref="VehicleType.Unknown"/>.
        /// </remarks>
        Task<IMEIToCallsign> GetFromIMEI(string imei);

        /// <summary>
        /// Asynchronously registers an IMEI to callsign relationship with the service.
        /// </summary>
        /// <param name="imei">The IMEI to register.</param>
        /// <param name="callsign">The associated callsign.</param>
        /// <param name="type">The type of vehicle the callsign is attached to.</param>
        Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null);
    }
}