using BikeTracker.Models.LocationModels;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BikeTracker.Models.Contexts
{
    public interface IIMEIContext
    {
        /// <summary>
        /// Gets the IMEI to callsigns relationships.
        /// </summary>
        /// <value>
        /// The IMEI to callsigns.
        /// </value>
        DbSet<IMEIToCallsign> IMEIToCallsigns { get; }
        /// <summary>
        /// Asynchronously saves the changes to the data context.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
