using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
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

        public async Task<IEnumerable<DateTimeOffset>> GetReportDates()
        {
            return await dataContext.LocationRecords.Select(l => DbFunctions.TruncateTime(l.ReadingTime)).Where(d => d.HasValue).Select(d => d.Value).Distinct().ToListAsync();
        }

        Task<IEnumerable<string>> IReportService.GetAllCallsigns()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<LocationRecord>> IReportService.GetCallsignRecord(string callsign, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<DateTimeOffset>> IReportService.GetReportDates()
        {
            throw new NotImplementedException();
        }
    }
}