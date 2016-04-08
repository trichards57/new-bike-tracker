﻿using BikeTracker.Models.LoggingModels;
using System;
using System.Linq;

namespace BikeTracker.Services
{
    public static class LogFormatter
    {
        public static string FormatLogEntry(LogEntry logEntry)
        {
            switch (logEntry.Type)
            {
                case LogEventType.UserLogIn:
                    return "logged in";
                case LogEventType.UserCreated:
                    var newUser = logEntry.Properties.First(p => p.PropertyType == LogPropertyType.Username);
                    return $"created account for {newUser.PropertyValue}";
                case LogEventType.UserUpdated:
                    var properties = logEntry.Properties.Where(p => p.PropertyType == LogPropertyType.PropertyChange).Select(p=>p.PropertyValue).OrderBy(s=>s).ToList();
                    string propString = string.Empty;
                    if (properties.Count == 1)
                        propString = properties[0];
                    else if (properties.Count == 2)
                        propString = $"{properties[0]} and {properties[1]}";
                    else if (properties.Count > 2)
                        propString = $"{string.Join(", ", properties.Take(properties.Count - 1))} and {properties.Last()}";

                    return $"changed {propString.ToLower()} for {logEntry.Properties.First(p=>p.PropertyType == LogPropertyType.Username).PropertyValue}";

                case LogEventType.UserDeleted:
                    var oldUser = logEntry.Properties.First(p => p.PropertyType == LogPropertyType.Username);
                    return $"deleted account for {oldUser.PropertyValue}";
                case LogEventType.IMEIRegistered:
                    var imei = logEntry.Properties.First(p => p.PropertyType == LogPropertyType.IMEI).PropertyValue;
                    var callsign = logEntry.Properties.First(p => p.PropertyType == LogPropertyType.Callsign).PropertyValue;
                    var vehicle = logEntry.Properties.First(p => p.PropertyType == LogPropertyType.VehicleType).PropertyValue;
                    return $"linked {imei} to callsign {callsign} with type {vehicle}";
                case LogEventType.IMEIDeleted:
                    var oldIMEI = logEntry.Properties.FirstOrDefault(p => p.PropertyType == LogPropertyType.IMEI).PropertyValue;
                    return $"deleted {oldIMEI}";
                case LogEventType.MapInUse:
                    var startDate = DateTimeOffset.Parse(logEntry.Properties.First(p => p.PropertyType == LogPropertyType.StartDate).PropertyValue);
                    return $"used the map between {startDate.ToString("g")} and {logEntry.Date.ToString("g")}";

                case LogEventType.UnknownEvent:
                default:
                    return "unknown event logged";
            }
        }
    }
}