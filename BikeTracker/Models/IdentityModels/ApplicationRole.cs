using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.IdentityModels
{
    /// <summary>
    /// Represents a role held by an <see cref="ApplicationUser"/>.
    /// </summary>
    /// <seealso cref="IdentityRole" />
    [ExcludeFromCodeCoverage]
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