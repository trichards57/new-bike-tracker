using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace BikeTracker.Models
{
    public partial class ApplicationDbContext
    {
        public DbSet<IMEIToCallsign> IMEIToCallsigns { get; set; }
        public DbSet<LocationRecord> LocationRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationRecord>().Property(o => o.Latitude).HasPrecision(18, 6);
            modelBuilder.Entity<LocationRecord>().Property(o => o.Longitude).HasPrecision(18, 6);
            modelBuilder.Entity<LocationRecord>().Property(o => o.Callsign).IsRequired().HasMaxLength(5);

            modelBuilder.Entity<IMEIToCallsign>().Property(o => o.IMEI).IsRequired();
            modelBuilder.Entity<IMEIToCallsign>().Property(o => o.CallSign).IsRequired().HasMaxLength(5);

            base.OnModelCreating(modelBuilder);
        }
    }

    public enum VehicleType
    {
        Unknown,
        Bike,
        [Display(Name ="Foot Patrol")]
        FootPatrol,
        Ambulance,
        Other
    }

    public class IMEIToCallsign
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        [Display(Name = "Call Sign")]
        public string CallSign { get; set; }
        public VehicleType Type { get; set; }
    }

    public class LocationRecord
    {
        public int Id { get; set; }
        public string Callsign { get; set; }
        public VehicleType Type { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTimeOffset ReadingTime { get; set; }
        public DateTimeOffset ReceiveTime { get; set; }
    }
}