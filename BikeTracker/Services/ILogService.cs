using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using System;
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
        Task LogIMEIRegistered(string registeringUser, string imei, string callsign, VehicleType type);

        /// <summary>
        /// Logs when an IMEI is deleted from the system.
        /// </summary>
        /// <param name="registeringUser">The registering user.</param>
        /// <param name="imei">The IMEI that was deleted.</param>
        Task LogIMEIDeleted(string registeringUser, string imei);

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
        /// Logs that a user's details are updated.
        /// </summary>
        /// <param name="updatingUser">The updating user.</param>
        /// <param name="changedProperties">The changed properties.</param>
        Task LogUserUpdated(string updatingUser, string updatedUser, IEnumerable<string> changedProperties);

        /// <summary>
        /// Logs when a user is using the map.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task LogMapInUse(string user);

        /// <summary>
        /// Asynchronously gets the log entries with on the given <paramref name="page" /> of size <paramref name="pageSize" />.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <remarks>
        /// If <paramref name="startDate"/> is null, the start date will be the beginning of the log.  If <paramref name="endDate"/>
        /// is null, the end date will be DateTimeOffset.Now.
        /// </remarks>
        /// <returns>An IEnumerable{LogEntry} containing the requested log entries.</returns>
        Task<IEnumerable<LogEntry>> GetLogEntries(int? pageSize = null, int? page = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null);
    }
}