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
        /// Displays the list of reports available to the user.
        /// </summary>
        /// <returns>A partial view usable by Angular</returns>
        public ActionResult Home()
        {
            return PartialView();
        }

        /// <summary>
        /// Displays the location reports plotted on a map.
        /// </summary>
        /// <returns>A partial view usable by Angular</returns>
        public ActionResult Locations()
        {
            return PartialView();
        }

        /// <summary>
        /// Displays the plots of location reporting rates.
        /// </summary>
        /// <returns>A partial view usable by Angular</returns>
        public ActionResult Rates()
        {
            return PartialView();
        }

        /// <summary>
        /// Displays the user activity log.
        /// </summary>
        /// <returns>A partial view usable by Angular</returns>
        public ActionResult UserActivity()
        {
            return PartialView();
        }
    }
}