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

    public class ReportService : IReportService
    {
        private ApplicationDbContext dataContext;

        public ReportService(ApplicationDbContext context)
        {
            dataContext = context;
        }

        public async Task<IEnumerable<string>> GetAllCallsigns()
        {
            return await dataContext.LocationRecords.Select(l => l.Callsign).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<LocationRecord>> GetCallsignRecord(string callsign, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var callsignRecords = dataContext.LocationRecords.Where(l => l.Callsign == callsign && l.ReadingTime >= startTime && l.ReadingTime <= endTime);
            return await callsignRecords.ToListAsync();
        }
    }
}
