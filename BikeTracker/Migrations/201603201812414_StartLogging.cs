namespace BikeTracker.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class StartLogging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        SourceUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogEntryProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogEntryId = c.Int(nullable: false),
                        PropertyType = c.Int(nullable: false),
                        PropertyValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LogEntries", t => t.LogEntryId, cascadeDelete: true)
                .Index(t => t.LogEntryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogEntryProperties", "LogEntryId", "dbo.LogEntries");
            DropIndex("dbo.LogEntryProperties", new[] { "LogEntryId" });
            DropTable("dbo.LogEntryProperties");
            DropTable("dbo.LogEntries");
        }
    }
}
