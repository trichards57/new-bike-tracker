using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public class IMEIService : IIMEIService
    {
        private ILocationIMEIContext dataContext;

        public IMEIService(ILocationIMEIContext context)
        {
            dataContext = context;
        }

        public async Task DeleteIMEI(string imei)
        {
            var callsign = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);

            await DeleteIMEI(callsign);
        }

        public async Task DeleteIMEIById(int id)
        {
            var callsign = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.Id == id);

            await DeleteIMEI(callsign);
        }

        public Task<IEnumerable<IMEIToCallsign>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<IMEIToCallsign>>(dataContext.IMEIToCallsigns);
        }

        public async Task<IMEIToCallsign> GetFromId(int id)
        {
            return await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<IQueryable<IMEIToCallsign>> GetFromIdQueryable(int id)
        {
            return Task.FromResult(dataContext.IMEIToCallsigns.Where(i => i.Id == id));
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

        public async Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null)
        {
            var iToC = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);

            if (iToC != null)
            {
                if (iToC.CallSign != callsign)
                {
                    var oldLocations = dataContext.LocationRecords.Where(l => l.Callsign == iToC.CallSign && l.Expired == false);

                    foreach (var l in oldLocations)
                        l.Expired = true;
                }

                iToC.CallSign = callsign ?? iToC.CallSign;
                iToC.Type = type ?? iToC.Type;
                await dataContext.SaveChangesAsync();
            }
            else
            {
                iToC = new IMEIToCallsign
                {
                    CallSign = callsign ?? "WR???",
                    IMEI = imei,
                    Type = type ?? VehicleType.Unknown
                };

                dataContext.IMEIToCallsigns.Add(iToC);
                await dataContext.SaveChangesAsync();
            }
        }

        private async Task DeleteIMEI(IMEIToCallsign imei)
        {
            if (imei == null)
                return;

            var oldLocations = dataContext.LocationRecords.Where(l => l.Callsign == imei.CallSign && l.Expired == false);

            foreach (var l in oldLocations)
                l.Expired = true;

            dataContext.IMEIToCallsigns.Remove(imei);
            await dataContext.SaveChangesAsync();
        }
    }
}