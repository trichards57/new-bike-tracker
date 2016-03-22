using BikeTracker.Controllers.Filters;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    [AuthorizePasswordExpires(Roles = "IMEIAdmin,GeneralAdmin")]
    public class ControlPanelController : Controller
    {
        public PartialViewResult Home()
        {
            return PartialView();
        }

        // GET: ControlPanel
        public ActionResult Index()
        {
            return View();
        }
    }
}