using BikeTracker.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace BikeTracker.Controllers.API
{
    [Authorize(Roles = "GeneralAdmin")]
    public class ReportController : ApiController
    {
        private IReportService reportService;

        public ReportController(IReportService service)
        {
            reportService = service;
        }

        public async Task<IHttpActionResult> GetCallsigns()
        {
            return Json(await reportService.GetAllCallsigns());
        }
    }
}
