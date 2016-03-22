﻿using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using BikeTracker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Tests.Services
{
    [TestClass]
    public class LogServiceTests
    {
        private const int TimeTolerance = 2;
        private readonly Fixture Fixture = new Fixture();
        private readonly List<string> TestChanges;
        private readonly string TestDeleteUser;
        private readonly string TestNewUser;
        private readonly string TestUsername;

        public LogServiceTests()
        {
            TestUsername = Fixture.Create<string>();
            TestNewUser = Fixture.Create<string>();
            TestDeleteUser = Fixture.Create<string>();
            TestChanges = new List<string>(Fixture.CreateMany<string>());
        }

        public Mock<ILoggingContext> CreateLoggingContext(DbSet<LogEntry> logEntrySet, DbSet<LogEntryProperty> logPropertySet)
        {
            var context = new Mock<ILoggingContext>(MockBehavior.Strict);

            context.SetupGet(c => c.LogEntries).Returns(logEntrySet);
            context.SetupGet(c => c.LogProperties).Returns(logPropertySet);
            context.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            return context;
        }

        public Mock<DbSet<LogEntry>> CreateMockLogEntrySet()
        {
            var mockLogEntrySet = new Mock<DbSet<LogEntry>>();

            return mockLogEntrySet;
        }

        public Mock<DbSet<LogEntryProperty>> CreateMockLogPropertySet()
        {
            var mockLogPropertySet = new Mock<DbSet<LogEntryProperty>>();

            return mockLogPropertySet;
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserCreatedEmptyNewUser()
        {
            await LogUserCreated(TestUsername, string.Empty, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserCreatedEmptySourceUser()
        {
            await LogUserCreated(string.Empty, TestNewUser, false);
        }

        [TestMethod]
        public async Task LogUserCreatedGoodData()
        {
            await LogUserCreated(TestUsername, TestNewUser);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserCreatedNoNewUser()
        {
            await LogUserCreated(TestUsername, null, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserCreatedNoSourceUser()
        {
            await LogUserCreated(null, TestNewUser, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserDeletedEmptyDeletedUser()
        {
            await LogUserDeleted(TestUsername, string.Empty, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserDeletedEmptySourceUser()
        {
            await LogUserDeleted(string.Empty, TestDeleteUser, false);
        }

        [TestMethod]
        public async Task LogUserDeletedGoodData()
        {
            await LogUserDeleted(TestUsername, TestDeleteUser);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserDeletedNoDeletedUser()
        {
            await LogUserDeleted(TestUsername, null, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserDeletedNoSourceUser()
        {
            await LogUserDeleted(null, TestDeleteUser, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserUpdateEmptyChanges()
        {
            await LogUserUpdates(TestUsername, Enumerable.Empty<string>(), false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserUpdateEmptySourceUser()
        {
            await LogUserUpdates(string.Empty, TestChanges);
        }

        [TestMethod]
        public async Task LogUserUpdateGoodData()
        {
            await LogUserUpdates(TestUsername, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserUpdateNoSourceUser()
        {
            await LogUserUpdates(null, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserUpdateNullChanges()
        {
            await LogUserUpdates(TestUsername, null, false);
        }

        private bool CheckImeiRegisteredLogEntry(LogEntry entry, string registeringUser, string imei, string callsign, VehicleType type)
        {
            CheckLogEntry(entry, registeringUser, new LogEntryProperty { PropertyType = LogPropertyType.Imei, PropertyValue = imei },
                new LogEntryProperty { PropertyValue = callsign, PropertyType = LogPropertyType.Callsign },
                new LogEntryProperty { PropertyType = LogPropertyType.VehicleType, PropertyValue = type.ToString() });

            return true;
        }

        private void CheckLogEntry(LogEntry entry, string sourceUser, params LogEntryProperty[] properties)
        {
            Assert.AreEqual(sourceUser, entry.SourceUser);
            Assert.AreEqual(LogEventType.UserCreated, entry.Type);

            foreach (var p in properties)
                Assert.IsNotNull(entry.Properties.SingleOrDefault(lep => lep.PropertyType == p.PropertyType && lep.PropertyValue == p.PropertyValue));

            Assert.AreEqual(properties.Length, entry.Properties.Count);
            Assert.IsTrue(Math.Abs((entry.Date - DateTimeOffset.Now).TotalSeconds) < TimeTolerance);
        }

        private bool CheckUserCreatedLogEntry(LogEntry entry, string creatingUser, string newUser)
        {
            CheckLogEntry(entry, creatingUser, new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = newUser });
            return true;
        }

        private bool CheckUserDeletedLogEntry(LogEntry entry, string deletingUser, string oldUser)
        {
            CheckLogEntry(entry, deletingUser, new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = oldUser });
            return true;
        }

        private bool CheckUserUpdatedLogEntry(LogEntry entry, string updatingUser, IEnumerable<string> changes)
        {
            var properties = changes.Select(c => new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = c }).ToArray();

            CheckLogEntry(entry, updatingUser, properties);
            return true;
        }

        private async Task LogImeiRegistered(string registeringUser, string imei, string callsign, VehicleType type, bool expectSuccess = true)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogImeiRegistered(registeringUser, imei, callsign, type);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => le.SourceUser == registeringUser
                      && le.Type == LogEventType.ImeiRegistered
                      && le.Properties.Count(lep => lep.PropertyType == LogPropertyType.Imei && lep.PropertyValue == imei) == 1
                      && le.Properties.Count(lep => lep.PropertyType == LogPropertyType.Callsign && lep.PropertyValue == callsign) == 1
                      && le.Properties.Count(lep => lep.PropertyType == LogPropertyType.VehicleType && lep.PropertyValue == type.ToString()) == 1
                      && le.Properties.Count == 3
                      && (le.Date - DateTimeOffset.Now).TotalSeconds < TimeTolerance)));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task LogUserCreated(string creatingUser, string newUser, bool expectSuccess = true)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogUserCreated(creatingUser, newUser);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckUserCreatedLogEntry(le, creatingUser, newUser))));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task LogUserDeleted(string deletingUser, string deletedUser, bool expectSuccess = true)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogUserDeleted(deletingUser, deletedUser);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckUserDeletedLogEntry(le, deletedUser, deletedUser))));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task LogUserUpdates(string updatingUser, IEnumerable<string> changes, bool expectSuccess = true)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogUserUpdated(updatingUser, changes);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckUserUpdatedLogEntry(le, updatingUser, changes))));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }
    }
}