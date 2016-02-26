using BikeTracker.Controllers.Filters;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// Controller that handles the home views which are open to everyone.
    /// </summary>
    [AllowAnonymousPasswordExpires]
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the top level Home View
        /// </summary>
        /// <returns>The Index view</returns>
        /// @mapping GET /
        /// @mapping GET /Home/
        /// @mapping GET /Home/Index
        /// @anon
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the contact details for the website
        /// </summary>
        /// <returns>The Contact view</returns>
        /// @mapping GET /Home/Contact
        /// @anon
        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Displays the privacy and image policies for the website
        /// </summary>
        /// <returns>The Policies view</returns>
        /// @mapping GET /Home/ Policies
        /// @anon
        public ActionResult Policies()
        {
            return View();
        }
    }
}