using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    public class DialogController : Controller
    {
        [HttpGet]
        public ActionResult DeleteForm()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ErrorForm()
        {
            return PartialView();
        }
    }
}