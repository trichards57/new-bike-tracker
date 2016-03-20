using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface ILogService
    {
        Task LogUserLoggedIn(string username);
        Task LogUserCreated(string creatingUser, string newUser);
        Task LogUserUpdated(string updatingUser, IEnumerable<string> changedProperties);
        Task LogUserDeleted(string deletingUser, string deletedUser);
    }
}
