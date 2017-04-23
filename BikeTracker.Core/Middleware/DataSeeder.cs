using BikeTracker.Core.Data;
using BikeTracker.Core.Models;
using BikeTracker.Core.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Core.Middleware
{
    [ExcludeFromCodeCoverage]
    public static class DataSeeder
    {
        public static void EnsureSeedData(this ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
            }
        }

        public static async Task EnsureSeedUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<DataSeederSettings> settingsGetter)
        {
            var settings = settingsGetter.Value;

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole { Name = RoleStrings.AdminRole });
            }
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    Email = settings.DefaultUserEmail,
                    UserName = settings.DefaultUserEmail,
                    RealName = settings.DefaultUserName,
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(user, settings.DefaultUserPassword);
                await userManager.AddToRoleAsync(user, RoleStrings.AdminRole);
            }
        }
    }

    public class DataSeederSettings
    {
        public string DefaultUserEmail { get; set; }
        public string DefaultUserName { get; set; }
        public string DefaultUserPassword { get; set; }
    }
}
