using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Service to collect data for reporting.
    /// </summary>
    /// <seealso cref="BikeTracker.Services.IReportService" />
    public class ReportService : IReportService
    {
        /// <summary>
        /// The data context used to store the data.
        /// </summary>
        private ILocationContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService"/> class.
        /// </summary>
        /// <param name="context">The data context to store to.</param>
        public ReportService(ILocationContext context)
        {
            dataContext = context;
        }

        /// <summary>
        /// Asynchronously gets a collection of all callsigns with registered location data.
        /// </summary>
        /// <returns>
        /// An <seealso cref="IEnumerable{string}" /> containing the callsigns.
        /// </returns>
        public async Task<IEnumerable<string>> GetAllCallsigns()
        {
            return await dataContext.LocationRecords.Select(l => l.Callsign).Distinct().ToListAsync();
        }

        /// <summary>
        /// Asynchronously gets a collection of all the location reports associated with a callsign in the given <see cref="DateTimeOffset" /> range.
        /// </summary>
        /// <param name="callsign">The callsign to get the records for.</param>
        /// <param name="startTime">The time to start searching from.</param>
        /// <param name="endTime">The time to end searching at.</param>
        /// <returns>
        /// An <seealso cref="IEnumerable{LocationRecord}" /> containing any report made in the given period.
        /// </returns>
        public async Task<IEnumerable<LocationRecord>> GetCallsignRecord(string callsign, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            if (callsign == null)
                throw new ArgumentNullException(nameof(callsign));
            if (string.IsNullOrWhiteSpace(callsign))
                throw new ArgumentException("{0} cannot be null or whitespace", nameof(callsign));

            var callsignRecords = dataContext.LocationRecords.Where(l => l.Callsign == callsign && l.ReadingTime >= startTime && l.ReadingTime <= endTime);
            return await callsignRecords.ToListAsync();
        }

        public async Task<IEnumerable<CheckInRate>> GetCheckInRatesByHour(string callsign, DateTimeOffset date)
        {
            if (callsign == null)
                throw new ArgumentNullException(nameof(callsign));
            if (string.IsNullOrWhiteSpace(callsign))
                throw new ArgumentException("parameter cannot be null", nameof(callsign));

            var daysRecords = await dataContext.LocationRecords
                .Where(l => DbFunctions.TruncateTime(l.ReadingTime) == date.Date && l.Callsign == callsign).ToListAsync();

            var result = Enumerable.Range(0,24).Select(h => new CheckInRate
            {
                Time = date.Date.AddHours(h),
                Count = daysRecords.Count(l => l.ReadingTime.Hour == h)
            });

            return result;
        }
    }
}