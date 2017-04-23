using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BikeTracker.Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string RealName { get; set; }
    }
}
