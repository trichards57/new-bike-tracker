using BikeTracker.Models.IdentityModels;
using BikeTracker.Models.LocationModels;
using BikeTracker.Models.LoggingModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.Contexts
{
    /// <summary>
    /// Main database for the application.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityDbContext{BikeTracker.Models.IdentityModels.ApplicationUser, BikeTracker.Models.IdentityModels.ApplicationRole, System.String, Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin, Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole, Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim}" />
    /// <seealso cref="BikeTracker.Models.Contexts.ILocationContext" />
    /// <seealso cref="BikeTracker.Models.Contexts.ILoggingContext" />
    /// <seealso cref="BikeTracker.Models.Contexts.IIMEIContext" />
    [ExcludeFromCodeCoverage]
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        ILocationContext, ILoggingContext, IIMEIContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
            : base(GetRDSConnection() ?? "DefaultConnection")
        {
        }

        public virtual DbSet<FailedLocationRecord> FailedRecords { get; set; }

        /// <summary>
        /// Gets the IMEI to callsigns relationships.
        /// </summary>
        /// <value>
        /// The IMEI to callsigns.
        /// </value>
        public virtual DbSet<IMEIToCallsign> IMEIToCallsigns { get; set; }

        /// <summary>
        /// Gets the landmarks.
        /// </summary>
        /// <value>
        /// The landmarks.
        /// </value>
        public virtual DbSet<Landmark> Landmarks { get; set; }

        /// <summary>
        /// Gets the location records.
        /// </summary>
        /// <value>
        /// The location records.
        /// </value>
        public virtual DbSet<LocationRecord> LocationRecords { get; set; }

        /// <summary>
        /// Gets the log entries.
        /// </summary>
        /// <value>
        /// The log entries.
        /// </value>
        public virtual DbSet<LogEntry> LogEntries { get; set; }

        /// <summary>
        /// Gets the log properties.
        /// </summary>
        /// <value>
        /// The log properties.
        /// </value>
        public virtual DbSet<LogEntryProperty> LogProperties { get; set; }

        /// <summary>
        /// Creates an instance of this data context.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Gets the RDS connection reported by the environment variables.
        /// </summary>
        /// <returns>A connection string for the RDS connection.</returns>
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

        /// <summary>
        /// Maps table names, and sets up relationships between the various user entities.
        /// </summary>
        /// <param name="modelBuilder">The builder used to create the database.</param>
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