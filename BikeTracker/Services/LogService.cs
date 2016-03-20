using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeTracker.Models.Contexts;
using BikeTracker.Models.LoggingModels;
using System.Linq;

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

        public async Task LogUserUpdated(string updatingUser, IEnumerable<string> changedProperties)
        {
            if (updatingUser == null)
                throw new ArgumentNullException(nameof(updatingUser));
            if (changedProperties == null)
                throw new ArgumentNullException(nameof(changedProperties));

            if (string.IsNullOrWhiteSpace(updatingUser))
                throw new ArgumentException("parameter cannot be empty", nameof(updatingUser));

            if (!changedProperties.Any())
                throw new ArgumentException("parameter cannot be empty", nameof(changedProperties));

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = updatingUser,
                Type = LogEventType.UserUpdated
            };
            var logProperties = changedProperties.Select(c => new LogEntryProperty
            {
                PropertyType = LogPropertyType.PropertyChange,
                PropertyValue = c
            });

            foreach (var lp in logProperties)
                logEntry.Properties.Add(lp);

            context.LogEntries.Add(logEntry);
            await context.SaveChangesAsync();
        }
    }
}