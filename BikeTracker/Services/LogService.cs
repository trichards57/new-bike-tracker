using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Service to log events for the system.
    /// </summary>
    /// <seealso cref="BikeTracker.Services.ILogService" />
    public class LogService : ILogService
    {
        /// <summary>
        /// The data context used to store the data
        /// </summary>
        private ILoggingContext dataContext;

        public const int MapUseTimeout = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="context">The data context to store to.</param>
        public LogService(ILoggingContext context)
        {
            dataContext = context;
        }

        /// <summary>
        /// Logs when an IMEI is registered in the system.
        /// </summary>
        /// <param name="registeringUser">The registering user.</param>
        /// <param name="imei">The IMEI that was registered.</param>
        /// <param name="callsign">The callsign associated with the IMEI.</param>
        /// <param name="type">The vehicle type associated with the callsign.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task LogIMEIRegistered(string registeringUser, string imei, string callsign, VehicleType type)
        {
            if (registeringUser == null)
                throw new ArgumentNullException(nameof(registeringUser));
            if (string.IsNullOrWhiteSpace(registeringUser))
                throw new ArgumentException("{0} cannot be null or whitespace", nameof(registeringUser));

            if (imei == null)
                throw new ArgumentNullException(nameof(imei));
            if (string.IsNullOrWhiteSpace(imei))
                throw new ArgumentException("{0} cannot be null or whitespace", nameof(imei));

            if (callsign == null)
                throw new ArgumentNullException(nameof(callsign));
            if (string.IsNullOrWhiteSpace(callsign))
                throw new ArgumentException("{0} cannot be null or whitespace", nameof(callsign));

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = registeringUser,
                Type = LogEventType.IMEIRegistered,
            };

            logEntry.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.IMEI, PropertyValue = imei });
            logEntry.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.Callsign, PropertyValue = callsign });
            logEntry.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.VehicleType, PropertyValue = type.ToString() });

            dataContext.LogEntries.Add(logEntry);
            await dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Logs when a user is created.
        /// </summary>
        /// <param name="creatingUser">The creating user.</param>
        /// <param name="newUser">The new user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// parameter cannot be empty
        /// or
        /// parameter cannot be empty
        /// </exception>
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
                PropertyType = LogPropertyType.Username,
                PropertyValue = newUser
            };
            logEntry.Properties.Add(logProperty);

            dataContext.LogEntries.Add(logEntry);
            await dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Logs when a user is deleted.
        /// </summary>
        /// <param name="deletingUser">The deleting user.</param>
        /// <param name="deletedUser">The deleted user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// parameter cannot be empty
        /// or
        /// parameter cannot be empty
        /// </exception>
        public async Task LogUserDeleted(string deletingUser, string deletedUser)
        {
            if (deletingUser == null)
                throw new ArgumentNullException(nameof(deletingUser));
            if (deletedUser == null)
                throw new ArgumentNullException(nameof(deletedUser));

            if (string.IsNullOrWhiteSpace(deletingUser))
                throw new ArgumentException("parameter cannot be empty", nameof(deletingUser));

            if (string.IsNullOrWhiteSpace(deletedUser))
                throw new ArgumentException("parameter cannot be empty", nameof(deletedUser));

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = deletingUser,
                Type = LogEventType.UserDeleted
            };
            var logProperty = new LogEntryProperty
            {
                PropertyType = LogPropertyType.Username,
                PropertyValue = deletedUser
            };
            logEntry.Properties.Add(logProperty);

            dataContext.LogEntries.Add(logEntry);
            await dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Logs when a user is logged in.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task LogUserLoggedIn(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("parameter cannot be empty", nameof(username));

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = username,
                Type = LogEventType.UserLogIn
            };

            dataContext.LogEntries.Add(logEntry);
            await dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Logs the a user's details are updated.
        /// </summary>
        /// <param name="updatingUser">The updating user.</param>
        /// <param name="changedProperties">The changed properties.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// parameter cannot be empty
        /// or
        /// parameter cannot be empty
        /// </exception>
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

            dataContext.LogEntries.Add(logEntry);
            await dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Logs when an IMEI is deleted from the system.
        /// </summary>
        /// <param name="registeringUser">The registering user.</param>
        /// <param name="imei">The IMEI that was deleted.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// Raised if the <paramref name="registeringUser"/> or <paramref name="imei"/> are null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Raised if the <paramref name="registeringUser"/> or <paramref name="imei"/> are empty or whitespace.
        /// </exception>
        public async Task LogIMEIDeleted(string registeringUser, string imei)
        {
            if (registeringUser == null)
                throw new ArgumentNullException(nameof(registeringUser));
            if (string.IsNullOrWhiteSpace(registeringUser))
                throw new ArgumentException("{0} cannot be empty", nameof(registeringUser));
            if (imei == null)
                throw new ArgumentNullException(nameof(imei));
            if (string.IsNullOrWhiteSpace(imei))
                throw new ArgumentException("{0} cannot be empty", nameof(imei));

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = registeringUser,
                Type = LogEventType.IMEIDeleted
            };
            var logProperty = new LogEntryProperty
            {
                PropertyType = LogPropertyType.IMEI,
                PropertyValue = imei
            };
            logEntry.Properties.Add(logProperty);

            dataContext.LogEntries.Add(logEntry);
            await dataContext.SaveChangesAsync();
        }

        public async Task LogMapInUse(string user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("{0} cannot be empty", nameof(user));

            var logEntry = dataContext.LogEntries.OrderBy(l => l.Date).LastOrDefault(l => l.SourceUser == user && l.Type == LogEventType.MapInUse);

            if (logEntry != null)
            {
                var prop = logEntry.Properties.First(lp => lp.PropertyType == LogPropertyType.StartDate);

                var date = DateTimeOffset.ParseExact(prop.PropertyValue, "O", CultureInfo.InvariantCulture);

                if (date > DateTimeOffset.Now.AddMinutes(-MapUseTimeout))
                {
                    logEntry.Date = DateTimeOffset.Now;
                }
                else
                    logEntry = null;
            }

            if (logEntry == null)
            {
                logEntry = new LogEntry
                {
                    Date = DateTimeOffset.Now,
                    SourceUser = user,
                    Type = LogEventType.MapInUse
                };
                var logProperty = new LogEntryProperty
                {
                    PropertyType = LogPropertyType.StartDate,
                    PropertyValue = DateTimeOffset.Now.ToString("O")
                };
                logEntry.Properties.Add(logProperty);

                dataContext.LogEntries.Add(logEntry);
            }

            await dataContext.SaveChangesAsync();
        }
    }
}