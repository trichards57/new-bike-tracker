using BikeTracker.Models.LocationModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface IIMEIService
    {
        Task DeleteIMEI(string imei);

        Task DeleteIMEIById(int id);

        Task<IEnumerable<IMEIToCallsign>> GetAllAsync();

        Task<IMEIToCallsign> GetFromId(int id);

        Task<IQueryable<IMEIToCallsign>> GetFromIdQueryable(int id);

        Task<IMEIToCallsign> GetFromIMEI(string imei);

        Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null);
    }
}