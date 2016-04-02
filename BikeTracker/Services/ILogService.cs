using BikeTracker.Models.LocationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Interface for a service that performs logging of specific events.
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Logs when an IMEI is registered in the system.
        /// </summary>
        /// <param name="registeringUser">The registering user.</param>
        /// <param name="imei">The IMEI that was registered.</param>
        /// <param name="callsign">The callsign associated with the IMEI.</param>
        /// <param name="type">The vehicle type associated with the callsign.</param>
        Task LogImeiRegistered(string registeringUser, string imei, string callsign, VehicleType type);

        /// <summary>
        /// Logs when an IMEI is deleted from the system.
        /// </summary>
        /// <param name="registeringUser">The registering user.</param>
        /// <param name="imei">The IMEI that was deleted.</param>
        Task LogImeiDeleted(string registeringUser, string imei);

        /// <summary>
        /// Logs when a user is created.
        /// </summary>
        /// <param name="creatingUser">The creating user.</param>
        /// <param name="newUser">The new user.</param>
        Task LogUserCreated(string creatingUser, string newUser);

        /// <summary>
        /// Logs when a user is deleted.
        /// </summary>
        /// <param name="deletingUser">The deleting user.</param>
        /// <param name="deletedUser">The deleted user.</param>
        Task LogUserDeleted(string deletingUser, string deletedUser);

        /// <summary>
        /// Logs when a user is logged in.
        /// </summary>
        /// <param name="username">The username.</param>
        Task LogUserLoggedIn(string username);

        /// <summary>
        /// Logs the a user's details are updated.
        /// </summary>
        /// <param name="updatingUser">The updating user.</param>
        /// <param name="changedProperties">The changed properties.</param>
        Task LogUserUpdated(string updatingUser, IEnumerable<string> changedProperties);

        /// <summary>
        /// Logs the map in use.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task LogMapInUse(string user);
    }
}