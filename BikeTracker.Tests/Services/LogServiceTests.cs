using BikeTracker.Models.Contexts;
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
        private readonly Fixture Fixture = new Fixture();
        private readonly string TestUsername;
        private readonly string TestNewUser;
        private readonly string TestDeleteUser;
        private const int TimeTolerance = 2;
        private readonly List<string> TestChanges;

        public LogServiceTests()
        {
            TestUsername = Fixture.Create<string>();
            TestNewUser = Fixture.Create<string>();
            TestDeleteUser = Fixture.Create<string>();
            TestChanges = new List<string>(Fixture.CreateMany<string>());
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

        public Mock<ILoggingContext> CreateLoggingContext(DbSet<LogEntry> logEntrySet, DbSet<LogEntryProperty> logPropertySet)
        {
            var context = new Mock<ILoggingContext>(MockBehavior.Strict);

            context.SetupGet(c => c.LogEntries).Returns(logEntrySet);
            context.SetupGet(c => c.LogProperties).Returns(logPropertySet);
            context.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            return context;
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
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => le.SourceUser == creatingUser
                      && le.Type == LogEventType.UserCreated
                      && le.Properties.Count(lep => lep.PropertyType == LogPropertyType.Username && lep.PropertyValue == newUser) == 1
                      && le.Properties.Count == 1
                      && (le.Date - DateTimeOffset.Now).TotalSeconds < TimeTolerance)));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        [TestMethod]
        public async Task LogUserCreatedGoodData()
        {
            await LogUserCreated(TestUsername, TestNewUser);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserCreatedEmptySourceUser()
        {
            await LogUserCreated(string.Empty, TestNewUser, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserCreatedNoSourceUser()
        {
            await LogUserCreated(null, TestNewUser, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserCreatedEmptyNewUser()
        {
            await LogUserCreated(TestUsername, string.Empty, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserCreatedNoNewUser()
        {
            await LogUserCreated(TestUsername, null, false);
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
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => le.SourceUser == updatingUser
                      && le.Type == LogEventType.UserUpdated
                      && CheckChangedProperties(changes, le.Properties)
                      && (le.Date - DateTimeOffset.Now).TotalSeconds < TimeTolerance)));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }

        }

        private bool CheckChangedProperties(IEnumerable<string> expectedChanges, IEnumerable<LogEntryProperty> actualProperties)
        {
            Assert.AreEqual(expectedChanges.Count(), actualProperties.Count());

            foreach (var c in expectedChanges)
            {
                var p = actualProperties.First(a => a.PropertyValue == c);
                Assert.AreEqual(LogPropertyType.PropertyChange, p.PropertyType);
            }

            return true;
        }

        [TestMethod]
        public async Task LogUserUpdateGoodData()
        {
            await LogUserUpdates(TestUsername, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserUpdateEmptySourceUser()
        {
            await LogUserUpdates(string.Empty, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserUpdateNoSourceUser()
        {
            await LogUserUpdates(null, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserUpdateEmptyChanges()
        {
            await LogUserUpdates(TestUsername, Enumerable.Empty<string>(), false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserUpdateNullChanges()
        {
            await LogUserUpdates(TestUsername, null, false);
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
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => le.SourceUser == deletingUser
                      && le.Type == LogEventType.UserDeleted
                      && le.Properties.Count(lep => lep.PropertyType == LogPropertyType.Username && lep.PropertyValue == deletedUser) == 1
                      && le.Properties.Count == 1
                      && (le.Date - DateTimeOffset.Now).TotalSeconds < TimeTolerance)));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        [TestMethod]
        public async Task LogUserDeletedGoodData()
        {
            await LogUserDeleted(TestUsername, TestDeleteUser);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserDeletedEmptySourceUser()
        {
            await LogUserDeleted(string.Empty, TestDeleteUser, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserDeletedNoSourceUser()
        {
            await LogUserDeleted(null, TestDeleteUser, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserDeletedEmptyDeletedUser()
        {
            await LogUserDeleted(TestUsername, string.Empty, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserDeletedNoDeletedUser()
        {
            await LogUserDeleted(TestUsername, null, false);
        }

    }
}
