using BikeTracker.Controllers.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace BikeTracker
{
    /// <summary>
    /// Setup class to configure the website's routes.
    /// </summary>
    [IgnoreCoverage]
    public class RouteConfig
    {
        /// <summary>
        /// Registers the main website's routes.
        /// </summary>
        /// <param name="routes">The routes collection to add to.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}