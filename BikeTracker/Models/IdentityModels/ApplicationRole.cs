using Microsoft.AspNet.Identity.EntityFramework;

namespace BikeTracker.Models.IdentityModels
{
    public class ApplicationRole : IdentityRole
    {
        public string DisplayName { get; set; }
    }
}