using BikeTracker.Models.Contexts;
using BikeTracker.Services;
using Microsoft.Practices.Unity;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;

namespace BikeTracker
{
    [ExcludeFromCodeCoverage]
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IIMEIService, IMEIService>();
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<ILogService, LogService>();
            container.RegisterType<ApplicationDbContext>(new InjectionFactory(c => new ApplicationDbContext()));
            container.RegisterType<ILocationContext>(new InjectionFactory(c => c.Resolve<ApplicationDbContext>()));
            container.RegisterType<IIMEIContext>(new InjectionFactory(c => c.Resolve<ApplicationDbContext>()));
            container.RegisterType<ILoggingContext>(new InjectionFactory(c => c.Resolve<ApplicationDbContext>()));

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}