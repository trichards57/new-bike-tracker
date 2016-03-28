namespace BikeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    [ExcludeFromCodeCoverage]
    public partial class AddedMustResetPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MustResetPassword", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MustResetPassword");
        }
    }
}
