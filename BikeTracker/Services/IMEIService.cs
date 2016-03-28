using BikeTracker.Models.Contexts;
using BikeTracker.Models.LocationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Service to manage the relationship between IMEIs and callsigns.
    /// </summary>
    /// <seealso cref="BikeTracker.Services.IIMEIService" />
    public class IMEIService : IIMEIService
    {
        /// <summary>
        /// The data context used to store the data.
        /// </summary>
        private IIMEIContext dataContext;
        private ILocationService locationService;

        public static readonly string DefaultCallsign = "WR???";

        /// <summary>
        /// Initializes a new instance of the <see cref="IMEIService"/> class.
        /// </summary>
        /// <param name="context">The data context to store to.</param>
        public IMEIService(IIMEIContext context, ILocationService locationService)
        {
            dataContext = context;
            this.locationService = locationService;
        }

        /// <summary>
        /// Asynchronously deletes the provided IMEI from the service's records.
        /// </summary>
        /// <param name="imei">The IMEI to delete.</param>
        /// <returns></returns>
        /// This function will not report an error if the IMEI does not exist.
        public async Task DeleteIMEI(string imei)
        {
            var callsign = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);

            await DeleteIMEI(callsign);
        }

        /// <summary>
        /// Asynchronously deletes the IMEI associated with the provided <paramref name="id" /> from the service's records.
        /// </summary>
        /// <param name="id">The identifier for the IMEI entry.</param>
        /// <returns></returns>
        /// This function will not report an error if the IMEI does not exist.
        public async Task DeleteIMEIById(int id)
        {
            var callsign = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.Id == id);

            await DeleteIMEI(callsign);
        }

        /// <summary>
        /// Asynchronously gets all of the IMEI to callsign relationships.
        /// </summary>
        /// <returns>
        /// An IEnumerable&lt;IMEIToCallsign&gt; containing all of the currently registered relationships.
        /// </returns>
        public Task<IEnumerable<IMEIToCallsign>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<IMEIToCallsign>>(dataContext.IMEIToCallsigns);
        }

        /// <summary>
        /// Asynchronously gets the IMEI to callsign relationship identified by the provided <paramref name="id" />.
        /// </summary>
        /// <param name="id">The identifier for the IMEI entry.</param>
        /// <returns>
        /// If a relationship under the provided ID exists, it is returned in an IMEIToCallsign.
        /// Otherwise returns null.
        /// </returns>
        public async Task<IMEIToCallsign> GetFromId(int id)
        {
            return await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.Id == id);
        }

        /// <summary>
        /// Asynchronously gets the IMEI to callsign relationship identified by the provided <paramref name="id" />.
        /// </summary>
        /// <param name="id">The identifier for the IMEI entry.</param>
        /// <returns>
        /// If a relationship under the provided ID exists, it is returned in an IMEIToCallsign contained in an IQueryable.
        /// Otherwise returns an empty IQueryable.
        /// </returns>
        public Task<IQueryable<IMEIToCallsign>> GetFromIdQueryable(int id)
        {
            return Task.FromResult(dataContext.IMEIToCallsigns.Where(i => i.Id == id));
        }

        /// <summary>
        /// Asynchronously gets the IMEI to callsign relationship containing the provided <paramref name="imei" />.
        /// </summary>
        /// <param name="imei">The IMEI to search for.</param>
        /// <returns>
        /// Returns the relationship in an IMEIToCallsign.
        /// </returns>
        /// <remarks>
        /// If the relationship does not exist when called, it will be created.  It will be given an unknown callsign
        /// and the vehicle type will be set to <see cref="VehicleType.Unknown" />.
        /// </remarks>
        public async Task<IMEIToCallsign> GetFromIMEI(string imei)
        {
            if (imei == null)
                throw new ArgumentNullException(nameof(imei));
            if (string.IsNullOrWhiteSpace(imei))
                throw new ArgumentException("{0} cannot be empty or only whitespace.", nameof(imei));

            var iToC = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);

            if (iToC == null)
            {
                await RegisterCallsign(imei);
                iToC = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);
            }

            return iToC;
        }

        /// <summary>
        /// Asynchronously registers an IMEI to callsign relationship with the service.
        /// </summary>
        /// <param name="imei">The IMEI to register.</param>
        /// <param name="callsign">The associated callsign.</param>
        /// <param name="type">The type of vehicle the callsign is attached to.</param>
        /// <returns></returns>
        public async Task RegisterCallsign(string imei, string callsign = null, VehicleType? type = null)
        {
            if (imei == null)
                throw new ArgumentNullException(nameof(imei));

            if (string.IsNullOrWhiteSpace(imei))
                throw new ArgumentException("{0} must not be empty or only whitespace", nameof(imei));

            var iToC = await dataContext.IMEIToCallsigns.FirstOrDefaultAsync(i => i.IMEI == imei);

            if (iToC != null)
            {
                if (!string.IsNullOrWhiteSpace(callsign) && iToC.CallSign != callsign)
                {
                    await locationService.ExpireLocation(iToC.CallSign);
                }

                iToC.CallSign = string.IsNullOrWhiteSpace(callsign) ? iToC.CallSign : callsign;
                iToC.Type = type ?? iToC.Type;
                await dataContext.SaveChangesAsync();
            }
            else
            {
                iToC = new IMEIToCallsign
                {
                    CallSign = callsign ?? DefaultCallsign,
                    IMEI = imei,
                    Type = type ?? VehicleType.Unknown
                };

                dataContext.IMEIToCallsigns.Add(iToC);
                await dataContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes the provided IMEIToCallsign relationship from the data context.
        /// </summary>
        /// <param name="imei">The IMEI to delete.</param>
        /// This method will not return an error if the IMEI is null or does not exist. 
        /// It will also trigger any location reports in the data context associated with that callsign
        /// to be expired to try and cut down on duplicate reports.
        private async Task DeleteIMEI(IMEIToCallsign imei)
        {
            if (imei == null)
                return;

            await locationService.ExpireLocation(imei.CallSign);

            dataContext.IMEIToCallsigns.Remove(imei);
            await dataContext.SaveChangesAsync();
        }
    }
}