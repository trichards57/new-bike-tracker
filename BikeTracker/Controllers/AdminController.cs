using BikeTracker.Controllers.Filters;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// Controller that handles basic admin tasks.
    /// </summary>
    [AuthorizePasswordExpires(Roles = "GeneralAdmin")]
    public class AdminController : Controller
    {
        public ActionResult EditForm()
        {
            return PartialView();
        }

        public ActionResult Home()
        {
            return PartialView();
        }
    }
}