using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface IIMEIService
    {
        Task<IEnumerable<IMEIToCallsign>> GetAllAsync();
        Task<IMEIToCallsign> GetFromIMEI(string imei);
        Task<IMEIToCallsign> GetFromId(int id);
        Task<IQueryable<IMEIToCallsign>> GetFromIdQueryable(int id);
        Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null);
        Task DeleteIMEI(string imei);
        Task DeleteIMEIById(int id);
    }
}
