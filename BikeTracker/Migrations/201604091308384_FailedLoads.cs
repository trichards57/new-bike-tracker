namespace BikeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FailedLoads : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FailedLocationRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Callsign = c.String(),
                        Reason = c.Int(nullable: false),
                        ReceivedTime = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FailedLocationRecords");
        }
    }
}
