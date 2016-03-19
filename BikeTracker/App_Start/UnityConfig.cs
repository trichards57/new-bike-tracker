using BikeTracker.Controllers.Filters;
using BikeTracker.Models.Contexts;
using BikeTracker.Services;
using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Mvc;

namespace BikeTracker
{
    [IgnoreCoverage]
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IIMEIService, IMEIService>();
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<ApplicationDbContext>(new InjectionFactory(c => new ApplicationDbContext()));

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}
