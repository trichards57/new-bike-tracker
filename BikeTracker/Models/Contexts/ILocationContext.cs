using BikeTracker.Models.LocationModels;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BikeTracker.Models.Contexts
{
    /// <summary>
    /// An interface for a data context that can store location and IMEI details.
    /// </summary>
    public interface ILocationContext
    {
        DbSet<FailedLocationRecord> FailedRecords { get; }

        /// <summary>
        /// Gets the landmarks.
        /// </summary>
        /// <value>
        /// The landmarks.
        /// </value>
        DbSet<Landmark> Landmarks { get; }

        /// <summary>
        /// Gets the location records.
        /// </summary>
        /// <value>
        /// The location records.
        /// </value>
        DbSet<LocationRecord> LocationRecords { get; }

        /// <summary>
        /// Asynchronously saves the changes to the data context.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}