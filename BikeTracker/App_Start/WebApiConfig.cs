using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
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
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Web API configuration and services
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<IMEIToCallsign>("IMEI");
            builder.EntitySet<UserAdminModel>("User");

            builder.Namespace = "API";

            var updateEmail = builder.EntityType<UserAdminModel>().Action("UpdateEmail");
            updateEmail.Parameter<string>("email");

            var register = builder.EntityType<UserAdminModel>().Collection.Action("Register");
            register.Parameter<string>("email");
            register.Parameter<string>("role");

            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

        }
    }
}