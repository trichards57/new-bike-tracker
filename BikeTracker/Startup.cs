using BikeTracker.Migrations;
using BikeTracker.Models;
using BikeTracker.Models.Contexts;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartup(typeof(BikeTracker.Startup))]
namespace BikeTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
