﻿using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public const int MapUseTimeout = 30;

        private readonly TelemetryClient _client = new TelemetryClient();

        /// <summary>
        /// The data context used to store the data
        /// </summary>
        private readonly ILoggingContext _dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="context">The data context to store to.</param>
        public LogService(ILoggingContext context)
        {
            _dataContext = context;
        }

        public Task<IEnumerable<LogEntry>> GetLogEntries(int? pageSize = null, int? page = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            if (pageSize == null && page != null)
                throw new ArgumentNullException(nameof(pageSize));

            if (endDate < startDate)
                throw new ArgumentException("End Date must not be before Start Date", nameof(endDate));

            IEnumerable<LogEntry> result = _dataContext.LogEntries.Include(le => le.Properties).OrderBy(l => l.Date);

            if (startDate != null)
                result = result.Where(le => le.Date >= startDate.Value);

            if (endDate != null)
                result = result.Where(le => le.Date <= endDate.Value);

            if (pageSize != null)
                result = result.Skip((page ?? 0) * pageSize.Value).Take(pageSize.Value);

            return Task.FromResult(result);
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

            var telemetry = new EventTelemetry(nameof(LogEventType.IMEIDeleted))
            {
                Timestamp = DateTimeOffset.Now
            };
            telemetry.Properties["SourceUser"] = registeringUser;
            telemetry.Properties["IMEI"] = imei;

            _client.TrackEvent(telemetry);

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

            _dataContext.LogEntries.Add(logEntry);
            await _dataContext.SaveChangesAsync();
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

            var telemetry = new EventTelemetry(nameof(LogEventType.IMEIRegistered))
            {
                Timestamp = DateTimeOffset.Now
            };
            telemetry.Properties["SourceUser"] = registeringUser;
            telemetry.Properties["IMEI"] = imei;
            telemetry.Properties["Callsign"] = callsign;
            telemetry.Properties["VehicleType"] = type.ToString();

            _client.TrackEvent(telemetry);

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = registeringUser,
                Type = LogEventType.IMEIRegistered,
            };

            logEntry.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.IMEI, PropertyValue = imei });
            logEntry.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.Callsign, PropertyValue = callsign });
            logEntry.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.VehicleType, PropertyValue = type.ToString() });

            _dataContext.LogEntries.Add(logEntry);
            await _dataContext.SaveChangesAsync();
        }

        public async Task LogMapInUse(string user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("{0} cannot be empty", nameof(user));

            var telemetry = new EventTelemetry(nameof(LogEventType.MapInUse))
            {
                Timestamp = DateTimeOffset.Now
            };
            telemetry.Properties["SourceUser"] = user;

            _client.TrackEvent(telemetry);

            var logEntry = await _dataContext.LogEntries.OrderByDescending(l => l.Date).FirstOrDefaultAsync(l => l.SourceUser == user && l.Type == LogEventType.MapInUse);

            if (logEntry != null)
            {
                var prop = logEntry.Properties.FirstOrDefault(lp => lp.PropertyType == LogPropertyType.StartDate);

                if (prop == null)
                    logEntry = null;
                else
                {
                    var date = DateTimeOffset.ParseExact(prop.PropertyValue, "O", CultureInfo.InvariantCulture);

                    if (date > DateTimeOffset.Now.AddMinutes(-MapUseTimeout))
                    {
                        logEntry.Date = DateTimeOffset.Now;
                    }
                    else
                        logEntry = null;
                }
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

                _dataContext.LogEntries.Add(logEntry);
            }

            await _dataContext.SaveChangesAsync();
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

            var telemetry = new EventTelemetry(nameof(LogEventType.UserCreated))
            {
                Timestamp = DateTimeOffset.Now
            };
            telemetry.Properties["SourceUser"] = creatingUser;
            telemetry.Properties["Username"] = newUser;

            _client.TrackEvent(telemetry);

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

            _dataContext.LogEntries.Add(logEntry);
            await _dataContext.SaveChangesAsync();
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

            var telemetry = new EventTelemetry(nameof(LogEventType.UserDeleted))
            {
                Timestamp = DateTimeOffset.Now
            };
            telemetry.Properties["SourceUser"] = deletingUser;
            telemetry.Properties["Username"] = deletedUser;

            _client.TrackEvent(telemetry);

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

            _dataContext.LogEntries.Add(logEntry);
            await _dataContext.SaveChangesAsync();
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

            var telemetry = new EventTelemetry(nameof(LogEventType.UserLogIn))
            {
                Timestamp = DateTimeOffset.Now
            };
            telemetry.Properties["SourceUser"] = username;

            _client.TrackEvent(telemetry);

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = username,
                Type = LogEventType.UserLogIn
            };

            _dataContext.LogEntries.Add(logEntry);
            await _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Logs the a user's details are updated.
        /// </summary>
        /// <param name="updatingUser">The updating user.</param>
        /// <param name="updatedUser">The user being updated</param>
        /// <param name="changedProperties">The changed properties.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// parameter cannot be empty
        /// or
        /// parameter cannot be empty
        /// </exception>
        public async Task LogUserUpdated(string updatingUser, string updatedUser, IEnumerable<string> changedProperties)
        {
            if (updatingUser == null)
                throw new ArgumentNullException(nameof(updatingUser));
            if (updatedUser == null)
                throw new ArgumentNullException(nameof(updatedUser));
            if (changedProperties == null)
                throw new ArgumentNullException(nameof(changedProperties));

            if (string.IsNullOrWhiteSpace(updatingUser))
                throw new ArgumentException("parameter cannot be empty", nameof(updatingUser));
            if (string.IsNullOrWhiteSpace(updatedUser))
                throw new ArgumentException("parameter cannot be empty", nameof(updatedUser));

            var properties = changedProperties as IList<string> ?? changedProperties.ToList();

            if (!properties.Any())
                throw new ArgumentException("parameter cannot be empty", nameof(changedProperties));

            var telemetry = new EventTelemetry(nameof(LogEventType.UserUpdated))
            {
                Timestamp = DateTimeOffset.Now
            };
            telemetry.Properties["SourceUser"] = updatingUser;
            telemetry.Properties["UpdatedProperties"] = string.Join(", ", changedProperties);
            telemetry.Properties["Username"] = updatedUser;

            _client.TrackEvent(telemetry);

            var logEntry = new LogEntry
            {
                Date = DateTimeOffset.Now,
                SourceUser = updatingUser,
                Type = LogEventType.UserUpdated
            };
            var logProperties = properties.Select(c => new LogEntryProperty
            {
                PropertyType = LogPropertyType.PropertyChange,
                PropertyValue = c
            });

            foreach (var lp in logProperties)
                logEntry.Properties.Add(lp);

            logEntry.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = updatedUser });

            _dataContext.LogEntries.Add(logEntry);
            await _dataContext.SaveChangesAsync();
        }
    }
}