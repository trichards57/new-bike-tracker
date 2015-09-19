using BikeTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    public interface IIMEIService
    {
        Task<IEnumerable<IMEIToCallsign>> GetAllAsync();
        Task<IMEIToCallsign> GetFromIMEI(string imei);
        Task<IMEIToCallsign> GetFromId(int id);
        Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null);
        Task DeleteIMEI(string imei);
        Task DeleteIMEIById(int id);
    }

    public class IMEIService : IIMEIService, IDisposable
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

        public async Task<IMEIToCallsign> GetFromId(int id)
        {
            return await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.Id == id);
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dataContext.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        public async Task DeleteIMEIById(int id)
        {
            var callsign = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.Id == id);
            if (callsign != null)
            {
                dataContext.IMEIToCallsigns.Remove(callsign);
                await dataContext.SaveChangesAsync();
            }

        }
    }
}
