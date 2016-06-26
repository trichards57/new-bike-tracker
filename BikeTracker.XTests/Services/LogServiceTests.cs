using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BikeTracker.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class LogServiceTests
    {
        private const int TestPageCount = 6;
        private const int TestPageSize = 5;
        private const int TimeTolerance = 2;
        private readonly Fixture Fixture = new Fixture();
        private readonly List<LogEntry> LogEntries;
        private readonly string TestCallsign;
        private readonly List<string> TestChanges;
        private readonly string TestDeleteUser;
        private readonly string TestIMEI;
        private readonly string TestNewUser;
        private readonly string TestUsername;

        public LogServiceTests()
        {
            TestUsername = Fixture.Create<string>();
            TestNewUser = Fixture.Create<string>();
            TestDeleteUser = Fixture.Create<string>();
            TestIMEI = Fixture.Create<string>();
            TestCallsign = Fixture.Create<string>();
            TestChanges = new List<string>(Fixture.CreateMany<string>());
            LogEntries = new List<LogEntry>();
        }

        public Mock<DbSet<LogEntry>> CreateMockLogEntrySet()
        {
            var mockLogEntrySet = new Mock<DbSet<LogEntry>>();

            var data = LogEntries.AsQueryable();

            mockLogEntrySet.As<IEnumerable<LogEntry>>().Setup(e => e.GetEnumerator()).Returns(data.GetEnumerator());

            mockLogEntrySet.As<IDbAsyncEnumerable<LogEntry>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<LogEntry>(data.GetEnumerator()));

            mockLogEntrySet.Setup(l => l.Include(It.IsAny<string>())).Returns(mockLogEntrySet.Object);

            mockLogEntrySet.As<IQueryable<LogEntry>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<LogEntry>(data.Provider));

            mockLogEntrySet.As<IQueryable<LogEntry>>().Setup(m => m.Expression).Returns(data.Expression);
            mockLogEntrySet.As<IQueryable<LogEntry>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockLogEntrySet.As<IQueryable<LogEntry>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockLogEntrySet;
        }

        [Fact]
        public async Task GetLogEntriesAfterDate()
        {
            var data = new List<LogEntry>();

            var expectedResult = new List<LogEntry>();

            for (var i = 1; i <= 10; i++)
                data.Add(Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.AddDays(-i).Date).Create());

            var le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.Date).Create();
            data.Add(le);
            expectedResult.Add(le);

            for (var i = 1; i <= 10; i++)
            {
                le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.AddDays(i).Date).Create();
                data.Add(le);
                expectedResult.Add(le);
            }

            await GetLogEntries(data, expectedResult, startDate: DateTimeOffset.Now.Date);
        }

        [Fact]
        public async Task GetLogEntriesBadDates()
        {
            var data = new List<LogEntry>();
            await Assert.ThrowsAsync<ArgumentException>(() => GetLogEntries(data, startDate: DateTimeOffset.Now.AddDays(1).Date, endDate: DateTimeOffset.Now.AddDays(-1).Date));
        }

        [Fact]
        public async Task GetLogEntriesBeforeDate()
        {
            var data = new List<LogEntry>();

            var expectedResult = new List<LogEntry>();

            LogEntry le;

            for (var i = 1; i <= 10; i++)
            {
                le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.AddDays(-i).Date).Create();
                data.Add(le);
                expectedResult.Add(le);
            }

            le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.Date).Create();
            data.Add(le);
            expectedResult.Add(le);

            for (var i = 1; i <= 10; i++)
            {
                le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.AddDays(i).Date).Create();
                data.Add(le);
            }

            await GetLogEntries(data, expectedResult, endDate: DateTimeOffset.Now.Date);
        }

        [Fact]
        public async Task GetLogEntriesBetweenDates()
        {
            var data = new List<LogEntry>();

            var expectedResult = new List<LogEntry>();

            const int testRange = 2;

            LogEntry le;

            for (var i = 1; i <= 10; i++)
            {
                le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.AddDays(-i).Date).Create();
                data.Add(le);
                if (i <= testRange)
                    expectedResult.Add(le);
            }

            le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.Date).Create();
            data.Add(le);
            expectedResult.Add(le);

            for (var i = 1; i <= 10; i++)
            {
                le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.AddDays(i).Date).Create();
                data.Add(le);
                if (i <= testRange)
                    expectedResult.Add(le);
            }

            await GetLogEntries(data, expectedResult, startDate: DateTimeOffset.Now.AddDays(-testRange).Date, endDate: DateTimeOffset.Now.AddDays(testRange).Date);
        }

        [Fact]
        public async Task GetLogEntriesDefaultData()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany().ToList();

            await GetLogEntries(data, data);
        }

        [Fact]
        public async Task GetLogEntriesFirstPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany(TestPageSize * TestPageCount).ToList();

            var expectedResult = data.OrderBy(t => t.Date).Take(TestPageSize).ToList();

            await GetLogEntries(data, expectedResult, TestPageSize, 0);
        }

        [Fact]
        public async Task GetLogEntriesNullPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany(TestPageSize * TestPageCount).ToList();

            var expectedResult = data.OrderBy(t => t.Date).Take(TestPageSize).ToList();

            await GetLogEntries(data, expectedResult, TestPageSize);
        }

        [Fact]
        public async Task GetLogEntriesNullPageSizeWithPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany().ToList();

            await Assert.ThrowsAsync<ArgumentNullException>(() => GetLogEntries(data, page: 0));
        }

        [Fact]
        public async Task GetLogEntriesThirdPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany(TestPageSize * TestPageCount).ToList();

            var expectedResult = data.OrderBy(t => t.Date).Skip(TestPageSize * 2).Take(TestPageSize).ToList();

            await GetLogEntries(data, expectedResult, TestPageSize, 2);
        }

        [Fact]
        public async Task LogIMEIDeletedEmptyIMEI()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogIMEIDeleted(TestUsername, string.Empty, false));
        }

        [Fact]
        public async Task LogIMEIDeletedEmptyUsername()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogIMEIDeleted(string.Empty, TestIMEI, false));
        }

        [Fact]
        public async Task LogIMEIDeletedGoodData()
        {
            await LogIMEIDeleted(TestUsername, TestIMEI);
        }

        [Fact]
        public async Task LogIMEIDeletedNoIMEI()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogIMEIDeleted(TestUsername, null, false));
        }

        [Fact]
        public async Task LogIMEIDeletedNoUsername()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogIMEIDeleted(null, TestIMEI, false));
        }

        [Fact]
        public async Task LogIMEIRegisteredEmptyCallsign()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogIMEIRegistered(TestUsername, TestIMEI, string.Empty, VehicleType.FootPatrol, false));
        }

        [Fact]
        public async Task LogIMEIRegisteredEmptyIMEI()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogIMEIRegistered(TestUsername, string.Empty, TestCallsign, VehicleType.FootPatrol, false));
        }

        [Fact]
        public async Task LogIMEIRegisteredEmptyUsername()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogIMEIRegistered(string.Empty, TestIMEI, TestCallsign, VehicleType.FootPatrol, false));
        }

        [Fact]
        public async Task LogIMEIRegisteredGoodData()
        {
            await LogIMEIRegistered(TestUsername, TestIMEI, TestCallsign, VehicleType.FootPatrol);
        }

        [Fact]
        public async Task LogIMEIRegisteredNoCallsign()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogIMEIRegistered(TestUsername, TestIMEI, null, VehicleType.FootPatrol, false));
        }

        [Fact]
        public async Task LogIMEIRegisteredNoIMEI()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogIMEIRegistered(TestUsername, null, TestCallsign, VehicleType.FootPatrol, false));
        }

        [Fact]
        public async Task LogIMEIRegisteredNoUsername()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogIMEIRegistered(null, TestIMEI, TestCallsign, VehicleType.FootPatrol, false));
        }

        [Fact]
        public async Task LogMapInUseEmptyName()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogMapInUse(string.Empty, false));
        }

        [Fact]
        public async Task LogMapInUseNewGoodData()
        {
            LogEntries.Clear();
            await LogMapInUse(TestUsername);
        }

        [Fact]
        public async Task LogMapInUseNullName()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogMapInUse(null, false));
        }

        [Fact]
        public async Task LogMapInUsePreviousGoodData()
        {
            LogEntries.Clear();

            var startDate = DateTimeOffset.Now.AddMinutes(-(LogService.MapUseTimeout + 1));

            var le = new LogEntry { Date = startDate, SourceUser = TestUsername, Type = LogEventType.MapInUse };
            le.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.StartDate, PropertyValue = startDate.ToString("O") });

            LogEntries.Add(le);

            await LogMapInUse(TestUsername);
        }

        [Fact]
        public async Task LogMapInUseUpdateGoodData()
        {
            LogEntries.Clear();

            var startDate = DateTimeOffset.Now.AddMinutes(-(LogService.MapUseTimeout - 1));

            var le = new LogEntry { Date = startDate, SourceUser = TestUsername, Type = LogEventType.MapInUse };
            le.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.StartDate, PropertyValue = startDate.ToString("O") });

            LogEntries.Add(le);

            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogMapInUse(TestUsername);

            logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
            context.Verify(c => c.SaveChangesAsync());

            CheckMapLogEntry(le, TestUsername, startDate);
        }

        [Fact]
        public async Task LogUserCreatedEmptyNewUser()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserCreated(TestUsername, string.Empty, false));
        }

        [Fact]
        public async Task LogUserCreatedEmptySourceUser()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserCreated(string.Empty, TestNewUser, false));
        }

        [Fact]
        public async Task LogUserCreatedGoodData()
        {
            await LogUserCreated(TestUsername, TestNewUser);
        }

        [Fact]
        public async Task LogUserCreatedNoNewUser()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserCreated(TestUsername, null, false));
        }

        [Fact]
        public async Task LogUserCreatedNoSourceUser()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserCreated(null, TestNewUser, false));
        }

        [Fact]
        public async Task LogUserDeletedEmptyDeletedUser()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserDeleted(TestUsername, string.Empty, false));
        }

        [Fact]
        public async Task LogUserDeletedEmptySourceUser()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserDeleted(string.Empty, TestDeleteUser, false));
        }

        [Fact]
        public async Task LogUserDeletedGoodData()
        {
            await LogUserDeleted(TestUsername, TestDeleteUser);
        }

        [Fact]
        public async Task LogUserDeletedNoDeletedUser()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserDeleted(TestUsername, null, false));
        }

        [Fact]
        public async Task LogUserDeletedNoSourceUser()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserDeleted(null, TestDeleteUser, false));
        }

        [Fact]
        public async Task LogUserLoggedInEmptyName()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserLoggedIn(string.Empty, false));
        }

        [Fact]
        public async Task LogUserLoggedInGoodData()
        {
            await LogUserLoggedIn(TestUsername, true);
        }

        [Fact]
        public async Task LogUserLoggedInNullName()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserLoggedIn(null, false));
        }

        [Fact]
        public async Task LogUserUpdateEmptyChanges()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserUpdates(TestUsername, TestNewUser, Enumerable.Empty<string>(), false));
        }

        [Fact]
        public async Task LogUserUpdateEmptySourceUser()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserUpdates(string.Empty, TestNewUser, TestChanges));
        }

        [Fact]
        public async Task LogUserUpdateEmptyUpdatedUser()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => LogUserUpdates(TestUsername, string.Empty, TestChanges));
        }

        [Fact]
        public async Task LogUserUpdateGoodData()
        {
            await LogUserUpdates(TestUsername, TestNewUser, TestChanges);
        }

        [Fact]
        public async Task LogUserUpdateNoSourceUser()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserUpdates(null, TestNewUser, TestChanges));
        }

        [Fact]
        public async Task LogUserUpdateNullChanges()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserUpdates(TestUsername, TestNewUser, null, false));
        }

        [Fact]
        public async Task LogUserUpdateNullUpdatedUser()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => LogUserUpdates(TestUsername, null, TestChanges));
        }

        private static void CheckLogEntry(LogEntry entry, LogEventType type, string sourceUser, params LogEntryProperty[] properties)
        {
            Assert.Equal(sourceUser, entry.SourceUser);
            Assert.Equal(type, entry.Type);

            foreach (var p in properties)
            {
                if (p.PropertyType == LogPropertyType.StartDate)
                    Assert.NotNull(entry.Properties.SingleOrDefault(lep => lep.PropertyType == p.PropertyType && Math.Abs((DateTimeOffset.ParseExact(lep.PropertyValue, "O", CultureInfo.InvariantCulture) - DateTimeOffset.ParseExact(p.PropertyValue, "O", CultureInfo.InvariantCulture)).TotalSeconds) < TimeTolerance));
                else
                    Assert.NotNull(entry.Properties.SingleOrDefault(lep => lep.PropertyType == p.PropertyType && lep.PropertyValue == p.PropertyValue));
            }

            Assert.Equal(properties.Length, entry.Properties.Count);
            Assert.True(Math.Abs((entry.Date - DateTimeOffset.Now).TotalSeconds) < TimeTolerance);
        }

        private static Mock<ILoggingContext> CreateLoggingContext(DbSet<LogEntry> logEntrySet, DbSet<LogEntryProperty> logPropertySet)
        {
            var context = new Mock<ILoggingContext>(MockBehavior.Strict);

            context.SetupGet(c => c.LogEntries).Returns(logEntrySet);
            context.SetupGet(c => c.LogProperties).Returns(logPropertySet);
            context.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            return context;
        }

        private static Mock<DbSet<LogEntryProperty>> CreateMockLogPropertySet()
        {
            var mockLogPropertySet = new Mock<DbSet<LogEntryProperty>>();

            return mockLogPropertySet;
        }

        private bool CheckIMEIDeletedLogEntry(LogEntry entry, string deletingUser, string imei)
        {
            CheckLogEntry(entry, LogEventType.IMEIDeleted, deletingUser, new LogEntryProperty { PropertyType = LogPropertyType.IMEI, PropertyValue = imei });
            return true;
        }

        private bool CheckIMEIRegisteredLogEntry(LogEntry entry, string registeringUser, string imei, string callsign, VehicleType type)
        {
            CheckLogEntry(entry, LogEventType.IMEIRegistered, registeringUser, new LogEntryProperty { PropertyType = LogPropertyType.IMEI, PropertyValue = imei },
                new LogEntryProperty { PropertyValue = callsign, PropertyType = LogPropertyType.Callsign },
                new LogEntryProperty { PropertyType = LogPropertyType.VehicleType, PropertyValue = type.ToString() });

            return true;
        }

        private bool CheckMapLogEntry(LogEntry entry, string username, DateTimeOffset startDate)
        {
            CheckLogEntry(entry, LogEventType.MapInUse, username, new LogEntryProperty { PropertyType = LogPropertyType.StartDate, PropertyValue = startDate.ToString("O") });

            return true;
        }

        private bool CheckUserCreatedLogEntry(LogEntry entry, string creatingUser, string newUser)
        {
            CheckLogEntry(entry, LogEventType.UserCreated, creatingUser, new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = newUser });
            return true;
        }

        private bool CheckUserDeletedLogEntry(LogEntry entry, string deletingUser, string oldUser)
        {
            CheckLogEntry(entry, LogEventType.UserDeleted, deletingUser, new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = oldUser });
            return true;
        }

        private bool CheckUserLoggedInEntry(LogEntry entry, string username)
        {
            CheckLogEntry(entry, LogEventType.UserLogIn, username);
            return true;
        }

        private bool CheckUserUpdatedLogEntry(LogEntry entry, string updatingUser, string updatedUser, IEnumerable<string> changes)
        {
            var properties = changes.Select(c => new LogEntryProperty { PropertyType = LogPropertyType.PropertyChange, PropertyValue = c }).ToList();
            properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.Username, PropertyValue = updatedUser });

            CheckLogEntry(entry, LogEventType.UserUpdated, updatingUser, properties.ToArray());
            return true;
        }

        private async Task GetLogEntries(IEnumerable<LogEntry> dataSet, IEnumerable<LogEntry> expectedDataSet = null, int? pageSize = null, int? page = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            LogEntries.Clear();
            LogEntries.AddRange(dataSet);

            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            var result = await service.GetLogEntries(pageSize, page, startDate, endDate);

            if (expectedDataSet != null)
            {
                Assert.True(expectedDataSet.OrderBy(l => l.Date).SequenceEqual(result));
            }

            context.Verify(c => c.SaveChangesAsync(), Times.Never);
        }

        private async Task LogIMEIDeleted(string registeringUser, string imei, bool expectSuccess = true)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogIMEIDeleted(registeringUser, imei);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckIMEIDeletedLogEntry(le, registeringUser, imei))));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task LogIMEIRegistered(string registeringUser, string imei, string callsign, VehicleType type, bool expectSuccess = true)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogIMEIRegistered(registeringUser, imei, callsign, type);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckIMEIRegisteredLogEntry(le, registeringUser, imei, callsign, type))));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task LogMapInUse(string username, bool expectSuccess = true, DateTimeOffset? startDate = null)
        {
            startDate = startDate ?? DateTimeOffset.Now;

            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogMapInUse(username);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckMapLogEntry(le, username, startDate.Value))));
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
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckUserDeletedLogEntry(le, deletingUser, deletedUser))));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task LogUserLoggedIn(string username, bool expectSuccess)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            await service.LogUserLoggedIn(username);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckUserLoggedInEntry(le, username))));
                context.Verify(c => c.SaveChangesAsync());
            }
            else
            {
                logEntrySet.Verify(l => l.Add(It.IsAny<LogEntry>()), Times.Never);
                context.Verify(c => c.SaveChangesAsync(), Times.Never);
            }
        }

        private async Task LogUserUpdates(string updatingUser, string updatedUser, IEnumerable<string> changes, bool expectSuccess = true)
        {
            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            var changedProperties = changes as IList<string> ?? changes.ToList();
            await service.LogUserUpdated(updatingUser, updatedUser, changedProperties);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckUserUpdatedLogEntry(le, updatingUser, updatedUser, changedProperties))));
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