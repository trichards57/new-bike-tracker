using BikeTracker.Controllers.Filters;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BikeTracker.Models.IdentityModels
{
    /// <summary>
    /// Represents a role held by an <see cref="ApplicationUser"/>.
    /// </summary>
    /// <seealso cref="IdentityRole" />
    [IgnoreCoverage]
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Gets or sets the display name for the role.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }
    }
}