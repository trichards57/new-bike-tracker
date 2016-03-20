using BikeTracker.Models.Contexts;
using BikeTracker.Models.LoggingModels;
using BikeTracker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using System;
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

        public LogServiceTests()
        {
            TestUsername = Fixture.Create<string>();
            TestNewUser = Fixture.Create<string>();
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
                      && le.Properties.Count(lep => lep.PropertyType == LogPropertyType.NewUser && lep.PropertyValue == newUser) == 1
                      && le.Properties.Count == 1)));
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
    }
}
