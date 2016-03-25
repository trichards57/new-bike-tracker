using BikeTracker.Controllers.Filters;
using BikeTracker.Models.Contexts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BikeTracker.Models.IdentityModels
{
    /// <summary>
    /// Represents the store used to manage user details.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.UserStore{ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim}" />
    [IgnoreCoverage]
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserStore"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ApplicationUserStore(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}