using BikeTracker.Controllers.Filters;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// Controller that deals with all of the IMEI to Callsign activities
    /// </summary>
    [AuthorizePasswordExpires(Roles = "IMEIAdmin,GeneralAdmin")]
    public class IMEIController : Controller
    {
        public ActionResult Home()
        {
            return PartialView();
        }

        public ActionResult EditForm()
        {
            return PartialView();
        }
    }
}