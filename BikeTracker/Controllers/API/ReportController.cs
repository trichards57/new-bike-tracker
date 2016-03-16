using System.Threading.Tasks;
using System.Web.Http;

namespace BikeTracker.Controllers.API
{
    [Authorize(Roles = "GeneralAdmin")]
    public class ReportController : ApiController
    {
        public Task<IHttpActionResult> GetCallsigns()
        {

        }
    }
}
