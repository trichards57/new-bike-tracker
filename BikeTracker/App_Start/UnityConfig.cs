using System.Web.Mvc;
using Microsoft.Practices.Unity;
using BikeTracker.Services;
using BikeTracker.Models;
using System.Web.Http;

namespace BikeTracker
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IIMEIService, IMEIService>();
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<ApplicationDbContext>(new InjectionFactory(c => new ApplicationDbContext()));

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}
