using BikeTracker.Models.LocationModels;
using BikeTracker.Models.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Interface for a service that collects data for reports.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Asynchronously gets a collection of all callsigns with registered location data.
        /// </summary>
        /// <returns>An <seealso cref="IEnumerable{string}"/> containing the callsigns.</returns>
        Task<IEnumerable<string>> GetAllCallsigns();

        /// <summary>
        /// Asynchronously gets a collection of all the location reports associated with a callsign in the given <see cref="DateTimeOffset"/> range.
        /// </summary>
        /// <param name="callsign">The callsign to get the records for.</param>
        /// <param name="startTime">The time to start searching from.</param>
        /// <param name="endTime">The time to end searching at.</param>
        /// <returns>
        /// An <seealso cref="IEnumerable{LocationRecord}"/> containing any report made in the given period.
        /// </returns>
        Task<IEnumerable<LocationRecord>> GetCallsignRecord(string callsign, DateTimeOffset startTime, DateTimeOffset endTime);

        Task<IEnumerable<CheckInRate>> GetCheckInRatesByHour(string callsign, DateTimeOffset date);
    }
}