using BikeTracker.Core.Data;
using BikeTracker.Core.Models.LocationModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteHelpers;

namespace BikeTracker.Core.Services
{
    public class LocationRecordService : ItemService<ApplicationDbContext, LocationRecord>
    {
        public LocationRecordService(ApplicationDbContext database) : base(database)
        {
        }

        public override Task<IEnumerable<LocationRecord>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<LocationRecord>>(Database.LocationRecords);
        }
    }
}
