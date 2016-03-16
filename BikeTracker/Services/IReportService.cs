using BikeTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface IReportService
    {
        Task<IEnumerable<string>> GetAllCallsigns();
        Task<IEnumerable<LocationRecord>> GetCallsignRecord(string callsign, DateTimeOffset startTime, DateTimeOffset endTime);
    }

    public class ReportService : IReportService, IDisposable
    {
        private ApplicationDbContext dataContext = new ApplicationDbContext();

        public async Task<IEnumerable<string>> GetAllCallsigns()
        {
            return await dataContext.LocationRecords.Select(l => l.Callsign).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<LocationRecord>> GetCallsignRecord(string callsign, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var callsignRecords = dataContext.LocationRecords.Where(l => l.Callsign == callsign && l.ReadingTime >= startTime && l.ReadingTime <= endTime);
            return await callsignRecords.ToListAsync();
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dataContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
