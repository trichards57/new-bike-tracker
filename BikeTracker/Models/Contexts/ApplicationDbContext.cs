using BikeTracker.Controllers.Filters;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Models.LocationModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;

namespace BikeTracker.Models.Contexts
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    [IgnoreCoverage]
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        ILocationIMEIContext
    {
        public static string GetRDSConnection()
        {
            var appConfig = ConfigurationManager.AppSettings;

            string dbname = appConfig["RDS_DB_NAME"];

            if (string.IsNullOrEmpty(dbname)) return null;

            string username = appConfig["RDS_USERNAME"];
            string password = appConfig["RDS_PASSWORD"];
            string hostname = appConfig["RDS_HOSTNAME"];
            string port = appConfig["RDS_PORT"];

            return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }

        public ApplicationDbContext()
            : base(GetRDSConnection() ?? "DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<IMEIToCallsign> IMEIToCallsigns { get; set; }
        public virtual DbSet<LocationRecord> LocationRecords { get; set; }
        public virtual DbSet<Landmark> Landmarks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationRecord>().Property(o => o.Latitude).HasPrecision(18, 6);
            modelBuilder.Entity<LocationRecord>().Property(o => o.Longitude).HasPrecision(18, 6);
            modelBuilder.Entity<LocationRecord>().Property(o => o.Callsign).IsRequired().HasMaxLength(5);

            modelBuilder.Entity<IMEIToCallsign>().Property(o => o.IMEI).IsRequired();
            modelBuilder.Entity<IMEIToCallsign>().Property(o => o.CallSign).IsRequired().HasMaxLength(5);

            modelBuilder.Entity<Landmark>().Property(o => o.Latitude).HasPrecision(18, 6);
            modelBuilder.Entity<Landmark>().Property(o => o.Longitude).HasPrecision(18, 6);
            modelBuilder.Entity<Landmark>().Property(o => o.Name).IsRequired().HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }
    }
}