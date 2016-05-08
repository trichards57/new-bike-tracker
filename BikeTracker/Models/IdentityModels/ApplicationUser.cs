using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BikeTracker.Models.IdentityModels
{
    /// <summary>
    /// Represents a user who can use the website.
    /// </summary>
    /// <seealso cref="IdentityUser" />
    [ExcludeFromCodeCoverage]
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user must reset password on next log in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if they must reset their password; otherwise, <c>false</c>.
        /// </value>
        public bool MustResetPassword { get; set; }

        /// <summary>
        /// Generates the user identity asynchronously.
        /// </summary>
        /// <param name="manager">The user manager to support this activity.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}