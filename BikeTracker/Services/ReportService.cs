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
        private readonly ILocationContext _dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService"/> class.
        /// </summary>
        /// <param name="context">The data context to store to.</param>
        public ReportService(ILocationContext context)
        {
            _dataContext = context;
        }

        /// <summary>
        /// Asynchronously gets a collection of all callsigns with registered location data.
        /// </summary>
        /// <returns>
        /// An <seealso cref="IEnumerable{string}" /> containing the callsigns.
        /// </returns>
        public async Task<IEnumerable<string>> GetAllCallsigns()
        {
            return await _dataContext.LocationRecords.Select(l => l.Callsign).Distinct().ToListAsync();
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

            var callsignRecords = _dataContext.LocationRecords.Where(l => l.Callsign == callsign && l.ReadingTime >= startTime && l.ReadingTime <= endTime);
            return await callsignRecords.ToListAsync();
        }

        public async Task<IEnumerable<CheckInRateViewModel>> GetCheckInRatesByHour(string callsign, DateTimeOffset date)
        {
            if (callsign == null)
                throw new ArgumentNullException(nameof(callsign));
            if (string.IsNullOrWhiteSpace(callsign))
                throw new ArgumentException("parameter cannot be null", nameof(callsign));

            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var daysRecords = _dataContext.LocationRecords
                .Where(l => l.ReadingTime >= startDate && l.ReadingTime < endDate && l.Callsign.Equals(callsign));

            var daysList = await daysRecords.ToListAsync();

            var result = Enumerable.Range(0, 24).Select(h => new CheckInRateViewModel
            {
                Time = date.Date.AddHours(h),
                Count = daysList.Count(l => l.ReadingTime.Hour == h)
            });

            return result;
        }

        public Task<IEnumerable<SuccessRateViewModel>> GetSuccessRatesByHour(string callsign, DateTimeOffset date)
        {
            if (callsign == null)
                throw new ArgumentNullException(nameof(callsign));
            if (string.IsNullOrWhiteSpace(callsign))
                throw new ArgumentException("parameter cannot be null", nameof(callsign));

            var daysRecords = _dataContext.LocationRecords.Where(l => l.Callsign == callsign);
            var failRecords = _dataContext.FailedRecords.Where(l => l.Callsign == callsign);

            var result = Enumerable.Range(0, 24).Select(h => new { Start = date.Date.AddHours(h), End = date.Date.AddHours(h + 1) }).Select(h => new SuccessRateViewModel
            {
                Time = h.Start,
                SuccessCount = daysRecords.Count(l => l.ReadingTime >= h.Start && l.ReadingTime < h.End),
                FailureCount = failRecords.Count(l => l.ReceivedTime >= h.Start && l.ReceivedTime < h.End)
            });

            return Task.FromResult(result);
        }
    }
}