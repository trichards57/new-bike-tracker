using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeTracker.Models.Contexts;
using BikeTracker.Models.LoggingModels;

namespace BikeTracker.Services
{
    public class LogService : ILogService
    {
        private ILoggingContext context;

        public LogService(ILoggingContext context)
        {
            this.context = context;
        }

        public async Task LogUserCreated(string creatingUser, string newUser)
        {
            if (creatingUser == null)
                throw new ArgumentNullException(nameof(creatingUser));
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            if (string.IsNullOrWhiteSpace(creatingUser))
                throw new ArgumentException("parameter cannot be empty", nameof(creatingUser));

            if (string.IsNullOrWhiteSpace(newUser))
                throw new ArgumentException("parameter cannot be empty", nameof(newUser));


            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = creatingUser,
                Type = LogEventType.UserCreated
            };
            var logProperty = new LogEntryProperty
            {
                PropertyType = LogPropertyType.NewUser,
                PropertyValue = newUser
            };
            logEntry.Properties.Add(logProperty);

            context.LogEntries.Add(logEntry);
            await context.SaveChangesAsync();
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