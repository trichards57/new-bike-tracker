using BikeTracker.Core.Models.LocationModels;
using BikeTracker.Core.Models.LocationViewModels;
using WebsiteHelpers;
using WebsiteHelpers.Interfaces;

namespace BikeTracker.Core.Controllers.Api
{
    public class CallsignController : ItemController<CallsignRecord, CallsignRecordDetails, CallsignRecordCreate, CallsignRecordSummary>
    {
        public CallsignController(IItemService<CallsignRecord> service) : base(service)
        {
        }
    }
}
