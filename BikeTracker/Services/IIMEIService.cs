using BikeTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;

namespace BikeTracker.Services
{
    public interface IIMEIService
    {
        Task<IEnumerable<IMEIToCallsign>> GetAllAsync();
        Task<IMEIToCallsign> GetFromIMEI(string imei);
        Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null);
        Task DeleteIMEI(string imei);
    }

    public class IMEIService : IIMEIService
    {
        private ApplicationDbContext dataContext = new ApplicationDbContext();

        public async Task DeleteIMEI(string imei)
        {
            var callsign = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);
            if (callsign != null)
            {
                dataContext.IMEIToCallsigns.Remove(callsign);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null)
        {
            var iToC = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);

            if (iToC != null)
            {
                iToC.CallSign = callsign ?? iToC.CallSign;
                iToC.Type = type ?? iToC.Type;
                await dataContext.SaveChangesAsync();
            }
            else
            {
                iToC = new IMEIToCallsign
                {
                    CallSign = callsign,
                    IMEI = imei ?? "WR???",
                    Type = type ?? VehicleType.Unknown
                };

                dataContext.IMEIToCallsigns.Add(iToC);
                await dataContext.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<IMEIToCallsign>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<IMEIToCallsign>>(dataContext.IMEIToCallsigns);
        }

        public async Task<IMEIToCallsign> GetFromIMEI(string imei)
        {
            var iToC = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);

            if (iToC == null)
            {
                await RegisterCallsign(imei);
                iToC = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);
            }

            return iToC;
        }
    }
}
