using System.Web.Http;

namespace BikeTracker
{
    /// <summary>
    /// Setup class for the Web API elements of the website
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Adds the API into the provided configuration.
        /// </summary>
        /// <param name="config">The configuration to start with.</param>
        /// <remarks>
        /// Maps the API to api/{controller}/{id} and uses the Http Attribute routing.
        /// </remarks>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}