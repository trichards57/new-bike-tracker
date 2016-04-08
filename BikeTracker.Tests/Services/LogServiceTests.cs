using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using BikeTracker.Services;
using BikeTracker.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace BikeTracker.Tests.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
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

            var data = LogEntries.AsQueryable();

            mockLogEntrySet.As<IEnumerable<LogEntry>>().Setup(e => e.GetEnumerator()).Returns(data.GetEnumerator());

            mockLogEntrySet.As<IDbAsyncEnumerable<LogEntry>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<LogEntry>(data.GetEnumerator()));

            mockLogEntrySet.As<IQueryable<LogEntry>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<LogEntry>(data.Provider));

            mockLogEntrySet.As<IQueryable<LogEntry>>().Setup(m => m.Expression).Returns(data.Expression);
            mockLogEntrySet.As<IQueryable<LogEntry>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockLogEntrySet.As<IQueryable<LogEntry>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockLogEntrySet;
        }

        public Mock<DbSet<LogEntryProperty>> CreateMockLogPropertySet()
        {
            var mockLogPropertySet = new Mock<DbSet<LogEntryProperty>>();

            return mockLogPropertySet;
        }

        [TestMethod]
        public async Task GetLogEntriesAfterDate()
        {
            var data = new List<LogEntry>();

            var expectedResult = new List<LogEntry>();

            LogEntry le;

            for (var i = 1; i <= 10; i++)
                data.Add(Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.AddDays(-i).Date).Create());

            le = Fixture.Build<LogEntry>().Without(l => l.Properties).With(l => l.Date, DateTimeOffset.Now.Date).Create();
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

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task GetLogEntriesBadDates()
        {
            var data = new List<LogEntry>();
            await GetLogEntries(data, data, startDate: DateTimeOffset.Now.AddDays(1).Date, endDate: DateTimeOffset.Now.AddDays(-1).Date, expectSuccess: false);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public async Task GetLogEntriesDefaultData()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany().ToList();

            await GetLogEntries(data, data);
        }

        [TestMethod]
        public async Task GetLogEntriesFirstPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany(TestPageSize * TestPageCount).ToList();

            var expectedResult = data.OrderBy(t => t.Date).Take(TestPageSize).ToList();

            await GetLogEntries(data, expectedResult, TestPageSize, 0);
        }

        [TestMethod]
        public async Task GetLogEntriesNullPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany(TestPageSize * TestPageCount).ToList();

            var expectedResult = data.OrderBy(t => t.Date).Take(TestPageSize).ToList();

            await GetLogEntries(data, expectedResult, TestPageSize, null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLogEntriesNullPageSizeWithPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany().ToList();

            await GetLogEntries(data, null, null, 0, expectSuccess: false);
        }

        [TestMethod]
        public async Task GetLogEntriesThirdPage()
        {
            var data = Fixture.Build<LogEntry>().Without(l => l.Properties).CreateMany(TestPageSize * TestPageCount).ToList();

            var expectedResult = data.OrderBy(t => t.Date).Skip(TestPageSize * 2).Take(TestPageSize).ToList();

            await GetLogEntries(data, expectedResult, TestPageSize, 2);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogIMEIDeletedEmptyIMEI()
        {
            await LogIMEIDeleted(TestUsername, string.Empty, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogIMEIDeletedEmptyUsername()
        {
            await LogIMEIDeleted(string.Empty, TestIMEI, false);
        }

        [TestMethod]
        public async Task LogIMEIDeletedGoodData()
        {
            await LogIMEIDeleted(TestUsername, TestIMEI);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogIMEIDeletedNoIMEI()
        {
            await LogIMEIDeleted(TestUsername, null, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogIMEIDeletedNoUsername()
        {
            await LogIMEIDeleted(null, TestIMEI, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogIMEIRegisteredEmptyCallsign()
        {
            await LogIMEIRegistered(TestUsername, TestIMEI, string.Empty, VehicleType.FootPatrol, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogIMEIRegisteredEmptyIMEI()
        {
            await LogIMEIRegistered(TestUsername, string.Empty, TestCallsign, VehicleType.FootPatrol, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogIMEIRegisteredEmptyUsername()
        {
            await LogIMEIRegistered(string.Empty, TestIMEI, TestCallsign, VehicleType.FootPatrol, false);
        }

        [TestMethod]
        public async Task LogIMEIRegisteredGoodData()
        {
            await LogIMEIRegistered(TestUsername, TestIMEI, TestCallsign, VehicleType.FootPatrol);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogIMEIRegisteredNoCallsign()
        {
            await LogIMEIRegistered(TestUsername, TestIMEI, null, VehicleType.FootPatrol, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogIMEIRegisteredNoIMEI()
        {
            await LogIMEIRegistered(TestUsername, null, TestCallsign, VehicleType.FootPatrol, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogIMEIRegisteredNoUsername()
        {
            await LogIMEIRegistered(null, TestIMEI, TestCallsign, VehicleType.FootPatrol, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogMapInUseEmptyName()
        {
            await LogMapInUse(string.Empty, false);
        }

        [TestMethod]
        public async Task LogMapInUseNewGoodData()
        {
            LogEntries.Clear();
            await LogMapInUse(TestUsername, true);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogMapInUseNullName()
        {
            await LogMapInUse(null, false);
        }

        [TestMethod]
        public async Task LogMapInUsePreviousGoodData()
        {
            LogEntries.Clear();

            var startDate = DateTimeOffset.Now.AddMinutes(-(LogService.MapUseTimeout + 1));

            var le = new LogEntry { Date = startDate, SourceUser = TestUsername, Type = LogEventType.MapInUse };
            le.Properties.Add(new LogEntryProperty { PropertyType = LogPropertyType.StartDate, PropertyValue = startDate.ToString("O") });

            LogEntries.Add(le);

            await LogMapInUse(TestUsername, true);
        }

        [TestMethod]
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
        public async Task LogUserLoggedInEmptyName()
        {
            await LogUserLoggedIn(string.Empty, false);
        }

        [TestMethod]
        public async Task LogUserLoggedInGoodData()
        {
            await LogUserLoggedIn(TestUsername, true);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserLoggedInNullName()
        {
            await LogUserLoggedIn(null, false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserUpdateEmptyChanges()
        {
            await LogUserUpdates(TestUsername, TestNewUser, Enumerable.Empty<string>(), false);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserUpdateEmptySourceUser()
        {
            await LogUserUpdates(string.Empty, TestNewUser, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserUpdateNullUpdatedUser()
        {
            await LogUserUpdates(TestUsername, null, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public async Task LogUserUpdateEmptyUpdatedUser()
        {
            await LogUserUpdates(TestUsername, string.Empty, TestChanges);
        }

        [TestMethod]
        public async Task LogUserUpdateGoodData()
        {
            await LogUserUpdates(TestUsername, TestNewUser, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserUpdateNoSourceUser()
        {
            await LogUserUpdates(null, TestNewUser, TestChanges);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public async Task LogUserUpdateNullChanges()
        {
            await LogUserUpdates(TestUsername, TestNewUser, null, false);
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

        private void CheckLogEntry(LogEntry entry, LogEventType type, string sourceUser, params LogEntryProperty[] properties)
        {
            Assert.AreEqual(sourceUser, entry.SourceUser);
            Assert.AreEqual(type, entry.Type);

            foreach (var p in properties)
            {
                if (p.PropertyType == LogPropertyType.StartDate)
                    Assert.IsNotNull(entry.Properties.SingleOrDefault(lep => lep.PropertyType == p.PropertyType && Math.Abs((DateTimeOffset.ParseExact(lep.PropertyValue, "O", CultureInfo.InvariantCulture) - DateTimeOffset.ParseExact(p.PropertyValue, "O", CultureInfo.InvariantCulture)).TotalSeconds) < TimeTolerance));
                else
                    Assert.IsNotNull(entry.Properties.SingleOrDefault(lep => lep.PropertyType == p.PropertyType && lep.PropertyValue == p.PropertyValue));
            }

            Assert.AreEqual(properties.Length, entry.Properties.Count);
            Assert.IsTrue(Math.Abs((entry.Date - DateTimeOffset.Now).TotalSeconds) < TimeTolerance);
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

        private async Task GetLogEntries(IEnumerable<LogEntry> dataSet, IEnumerable<LogEntry> expectedDataSet, int? pageSize = null, int? page = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, bool expectSuccess = true)
        {
            LogEntries.Clear();
            LogEntries.AddRange(dataSet);

            var logEntrySet = CreateMockLogEntrySet();
            var logPropertySet = CreateMockLogPropertySet();
            var context = CreateLoggingContext(logEntrySet.Object, logPropertySet.Object);

            var service = new LogService(context.Object);

            var result = await service.GetLogEntries(pageSize, page, startDate, endDate);

            if (expectSuccess)
            {
                Assert.IsTrue(expectedDataSet.OrderBy(l => l.Date).SequenceEqual(result));
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

            await service.LogUserUpdated(updatingUser, updatedUser, changes);

            if (expectSuccess)
            {
                logEntrySet.Verify(l => l.Add(It.Is<LogEntry>(le => CheckUserUpdatedLogEntry(le, updatingUser, updatedUser, changes))));
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