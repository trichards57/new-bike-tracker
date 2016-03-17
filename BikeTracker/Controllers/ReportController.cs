using BikeTracker.Controllers.Filters;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// Controller to handle reporting on the activity on the tracker.
    /// </summary>
    [AuthorizePasswordExpires(Roles = "GeneralAdmin")]
    public class ReportController : Controller
    {
        /// <summary>
        /// Displays the list of reports available to the user
        /// </summary>
        public ActionResult Home()
        {
            return PartialView();
        }

        public ActionResult UserActivity()
        {
            return PartialView();
        }

        public ActionResult Locations()
        {
            return PartialView();
        }

        public ActionResult Rates()
        {
            return PartialView();
        }
    }
}