namespace BikeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public partial class LandmarksAndExpirary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Landmarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Expiry = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.LocationRecords", "Expired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocationRecords", "Expired");
            DropTable("dbo.Landmarks");
        }
    }
}
