using BikeTracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;

namespace BikeTracker.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BikeTracker.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {

            var ga = new ApplicationRole
            {
                Id = "164F8F72-1D1D-4EFF-BCF8-09181A2F2301",
                Name = "GeneralAdmin",
                DisplayName = "General Administrator"
            };
            context.Roles.AddOrUpdate(
                new ApplicationRole
                {
                    Id = "373BCB10-EDF9-418C-8C05-3289DDF92F4F",
                    Name = "Normal",
                    DisplayName = "Normal User"
                },
                ga,
                new ApplicationRole
                {
                    Id = "FEF5DD3C-BCF5-4A11-9430-237F077594A5",
                    Name = "IMEIAdmin",
                    DisplayName = "IMEI Administrator"
                }
            );

            var u = new ApplicationUser
            {
                Id = "7f1a442b-3c3c-4757-96b8-aaa2db377e91",
                Email = "tony.richards@sja.org.uk",
                PasswordHash = "ANelm9QxQnz8ks1U5SR6ydJAOZR8082O7hvKvFzHRBeEuFqpF9M0973tgtxphYmUnQ==",
                SecurityStamp = "58544c68-e03d-468f-838d-56cb86d017b6",
                UserName = "tony.richards@sja.org.uk",
            };
            u.Roles.Add(new IdentityUserRole { RoleId = ga.Id, UserId = u.Id });

            context.Users.AddOrUpdate(u);

            context.SaveChanges();
        }
    }
}
