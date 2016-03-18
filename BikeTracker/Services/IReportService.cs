﻿using BikeTracker.Models.LocationModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface IReportService
    {
        Task<IEnumerable<string>> GetAllCallsigns();
        Task<IEnumerable<DateTimeOffset>> GetReportDates();
        Task<IEnumerable<LocationRecord>> GetCallsignRecord(string callsign, DateTimeOffset startTime, DateTimeOffset endTime);
    }
}
