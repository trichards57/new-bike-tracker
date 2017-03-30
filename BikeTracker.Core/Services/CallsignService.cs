using BikeTracker.Core.Data;
using BikeTracker.Core.Models.LocationModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteHelpers;

namespace BikeTracker.Core.Services
{
    public class CallsignService : ItemService<ApplicationDbContext, CallsignRecord>
    {

        public CallsignService(ApplicationDbContext database) : base(database)
        {
        }

        public override Task<IEnumerable<CallsignRecord>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<CallsignRecord>>(Database.CallsignRecords);
        }
    }
}
