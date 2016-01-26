using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using BikeTracker.Models;


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
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<IMEIToCallsign>("IMEI");
            builder.EntitySet<UserAdminModel>("User");

            var updateEmail = builder.Entity<UserAdminModel>().Action("UpdateEmail");
            updateEmail.Parameter<string>("email");

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

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