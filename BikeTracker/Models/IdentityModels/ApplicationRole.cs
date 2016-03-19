using BikeTracker.Controllers.Filters;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BikeTracker.Models.IdentityModels
{
    [IgnoreCoverage]
    public class ApplicationRole : IdentityRole
    {
        public string DisplayName { get; set; }
    }
}