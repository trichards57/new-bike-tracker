using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using BikeTracker.Services;
using Ploeh.AutoFixture;
using System;
using Xunit;

namespace BikeTracker.Tests.Services
{
    public class LogFormatterTests
    {
        private readonly Fixture Fixture = new Fixture();

        [Fact]
        public void FormatIMEIDeleted()
        {
            var imei = Fixture.Create<string>();

            var le = new LogEntry
            {
                Type = LogEventType.IMEIDeleted,
                Properties = new[] {
                    new LogEntryProperty { PropertyType = LogPropertyType.IMEI, PropertyValue = imei },
                }
            };
            var expected = $"deleted {imei}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatIMEIRegistered()
        {
            var callsign = Fixture.Create<string>();
            var imei = Fixture.Create<string>();
            var vehicle = Fixture.Create<VehicleType>().ToString();

            var le = new LogEntry
            {
                Type = LogEventType.IMEIRegistered,
                Properties = new[] {
                    new LogEntryProperty { PropertyType = LogPropertyType.Callsign, PropertyValue = callsign },
                    new LogEntryProperty { PropertyType = LogPropertyType.IMEI, PropertyValue = imei },
                    new LogEntryProperty { PropertyType = LogPropertyType.VehicleType, PropertyValue = vehicle }
                }
            };
            var expected = $"linked {imei} to callsign {callsign} with type {vehicle}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatMapInUse()
        {
            var startDate = Fixture.Create<DateTimeOffset>();
            var endDate = startDate.AddHours(3);

            var le = new LogEntry
            {
                Date = endDate,
                Type = LogEventType.MapInUse,
                Properties = new[] {
                    new LogEntryProperty { PropertyType = LogPropertyType.StartDate, PropertyValue = startDate.ToString("O") },
                }
            };
            var expected = $"used the map between {startDate.ToString("g")} and {endDate.ToString("g")}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUnknownEvent()
        {
            var le = new LogEntry { Type = LogEventType.UnknownEvent };
            const string expected = "unknown event logged";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserCreated()
        {
            var name = Fixture.Create<string>();
            var le = new LogEntry { Type = LogEventType.UserCreated, Properties = new[] { new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = name } } };
            var expected = $"created account for {name}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserDeleted()
        {
            var name = Fixture.Create<string>();
            var le = new LogEntry { Type = LogEventType.UserDeleted, Properties = new[] { new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = name } } };
            var expected = $"deleted account for {name}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserLogIn()
        {
            var le = new LogEntry { Type = LogEventType.UserLogIn };
            const string expected = "logged in";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserUpdatedEmail()
        {
            var name = Fixture.Create<string>();
            var le = new LogEntry
            {
                Type = LogEventType.UserUpdated,
                Properties = new[] {
                    new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Email" },
                    new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = name }
                }
            };
            var expected = $"changed email for {name}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserUpdatedEmailAndPassword()
        {
            var name = Fixture.Create<string>();
            var le = new LogEntry
            {
                Type = LogEventType.UserUpdated,
                Properties = new[] {
                    new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Email" },
                    new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Password" },
                    new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = name }
                }
            };
            var expected = $"changed email and password for {name}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserUpdatedEmailRoleAndPassword()
        {
            var name = Fixture.Create<string>();
            var le = new LogEntry
            {
                Type = LogEventType.UserUpdated,
                Properties = new[] {
                    new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Role" },
                    new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Email" },
                    new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Password" },
                    new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = name }
                }
            };
            var expected = $"changed email, password and role for {name}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserUpdatedPassword()
        {
            var name = Fixture.Create<string>();
            var le = new LogEntry
            {
                Type = LogEventType.UserUpdated,
                Properties = new[] { new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Password" }, new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = name } }
            };
            var expected = $"changed password for {name}";
            FormatLogEntry(le, expected);
        }

        [Fact]
        public void FormatUserUpdatedRole()
        {
            var name = Fixture.Create<string>();
            var le = new LogEntry
            {
                Type = LogEventType.UserUpdated,
                Properties = new[] {
                    new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = "Role" },
                    new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = name }
                }
            };
            var expected = $"changed role for {name}";
            FormatLogEntry(le, expected);
        }

        private static void FormatLogEntry(LogEntry entry, string expectedString)
        {
            var result = LogFormatter.FormatLogEntry(entry);

            Assert.Equal(expectedString, result);
        }
    }
}