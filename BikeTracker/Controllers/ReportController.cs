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
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Report that shows all of the LocationRecords for given callsign
        /// </summary>
        public ActionResult LocationReport()
        {
            return View();
        }

        /// <summary>
        /// Report that shows the reliability measures for a given callsign
        /// </summary>
        /// <remarks>
        /// Measures include:
        /// * Number of location reports received on time
        /// * Number of location reports received that did not include a location
        /// </remarks>
        public ActionResult ReliabilityReport()
        {
            return View();
        }

        /// <summary>
        /// Report that shows all of the activities logged for a given user.
        /// </summary>
        public ActionResult UserActivityReport()
        {
            return View();
        }
    }
}