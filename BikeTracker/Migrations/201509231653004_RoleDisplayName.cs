namespace BikeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public partial class RoleDisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "DisplayName", c => c.String());


        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "DisplayName");
        }
    }
}
