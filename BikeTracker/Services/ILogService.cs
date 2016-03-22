using BikeTracker.Models.LocationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface ILogService
    {
        Task LogImeiRegistered(string registeringUser, string imei, string callsign, VehicleType type);

        Task LogUserCreated(string creatingUser, string newUser);

        Task LogUserDeleted(string deletingUser, string deletedUser);

        Task LogUserLoggedIn(string username);

        Task LogUserUpdated(string updatingUser, IEnumerable<string> changedProperties);
    }
}