using BikeTracker.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker
{
    public interface IUserManager
    {
        IIdentityValidator<string> PasswordValidator { get; }

        IQueryable<ApplicationUser> Users { get; }

        Task<IdentityResult> AddToRoleAsync(string userId, string role);

        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<IdentityResult> DeleteAsync(ApplicationUser user);

        Task<ApplicationUser> FindAsync(string username, string password);

        Task<ApplicationUser> FindByEmailAsync(string email);

        Task<ApplicationUser> FindByIdAsync(string userId);

        Task<ApplicationUser> FindByNameAsync(string userName);

        Task GenerateEmailConfirmationEmailAsync(UrlHelper url, string id);

        Task GenerateEmailConfirmationEmailAsync(UrlHelper url, string id, string temporaryPassword);

        Task GeneratePasswordResetEmailAsync(UrlHelper url, string id);

        Task<IList<string>> GetRolesAsync(string userId);

        Task<bool> IsEmailConfirmedAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] role);

        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);

        Task<IdentityResult> SetEmailAsync(string userId, string email);
    }
}