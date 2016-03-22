using BikeTracker.Models.LocationModels;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BikeTracker.Models.Contexts
{
    public interface ILocationIMEIContext
    {
        DbSet<IMEIToCallsign> IMEIToCallsigns { get; }
        DbSet<Landmark> Landmarks { get; }
        DbSet<LocationRecord> LocationRecords { get; }

        Task<int> SaveChangesAsync();
    }
}