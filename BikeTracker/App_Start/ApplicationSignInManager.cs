using BikeTracker.Models.IdentityModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BikeTracker
{
    // Configure the application sign-in manager which is used in this application.
    /// <summary>
    /// Subclass of SignInManager, set up to use <see cref="ApplicationUser"/> instead of IdentityUser.
    /// </summary>
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSignInManager"/> class.
        /// </summary>
        /// <param name="userManager">The user manager to use.</param>
        /// <param name="authenticationManager">The authentication manager to use.</param>
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        /// <summary>
        /// Creates an <see cref="ApplicationSignInManager"/>
        /// </summary>
        /// <param name="options">The options used to create the <see cref="ApplicationSignInManager"/>.</param>
        /// <param name="context">The context that supports the <see cref="ApplicationSignInManager"/>.</param>
        /// <returns>A new instance of <see cref="ApplicationSignInManager"/> configured using the specified options.</returns>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        /// <summary>
        /// Creates a ClaimsIdentity from the provided <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/>.</param>
        /// <returns>A ClaimsIdentity for the user.</returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
}