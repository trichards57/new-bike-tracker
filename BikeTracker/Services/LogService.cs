using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public class LogService : ILogService
    {
        public Task LogUserCreated(string creatingUser, string newUser)
        {
            throw new NotImplementedException();
        }

        public Task LogUserDeleted(string deletingUser, string deletedUser)
        {
            throw new NotImplementedException();
        }

        public Task LogUserLoggedIn(string username)
        {
            throw new NotImplementedException();
        }

        public Task LogUserUpdated(string updatingUser, IEnumerable<string> changedProperties)
        {
            throw new NotImplementedException();
        }
    }
}