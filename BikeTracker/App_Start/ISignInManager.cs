using BikeTracker.Models.IdentityModels;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace BikeTracker
{
    public interface ISignInManager
    {
        Task<SignInStatus> PasswordSignInAsync(string username, string password, bool isPersistent, bool shouldLockout);
        Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser);
    }
}
