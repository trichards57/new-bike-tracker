using BikeTracker.Models.Contexts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BikeTracker.Models.IdentityModels
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public ApplicationUserStore(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}