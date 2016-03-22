using BikeTracker.Controllers.Filters;
using BikeTracker.Models.Contexts;
using BikeTracker.Models.IdentityModels;
using BikeTracker.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker
{
    /// <summary>
    /// Subclass of UserManager, set up to use <see cref="ApplicationUser"/> instead of IdentityUser.
    /// Used to support user management for the website.
    /// </summary>
    [IgnoreCoverage]
    public class ApplicationUserManager : UserManager<ApplicationUser, string>, IUserManager
    {
        public const int MinPasswordLength = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
        /// </summary>
        /// <param name="store">The store used to keep user information.</param>
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ApplicationUserManager"/>.
        /// </summary>
        /// <param name="options">The options used to create the ApplicationUserManager.</param>
        /// <param name="context">The data context used to store the user information.</param>
        /// <returns>An <see cref="ApplicationUserManager"/> created using the given options.</returns>
        /// <remarks>
        /// Sets up the following basic options:
        ///
        /// * Non-alphanumeric usernames are allowed
        /// * User email addresses must be unique
        /// * Passwords must be at least <see cref="MinPasswordLength"/> characters long, including a digit, a symbol and upper- and lower-case letters
        /// * User lockout is enabled and defaults to 5 minutes lockout after 5 failed attempts
        /// * <see cref="EmailService"/> is used to send emails
        /// * DataProtectorTokenProvider is used to generate email confirmation tokens
        /// </remarks>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = MinPasswordLength,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var res = await base.ChangePasswordAsync(userId, currentPassword, newPassword);

            if (res.Succeeded)
            {
                var user = await Store.FindByIdAsync(userId);
                user.MustResetPassword = false;
                await Store.UpdateAsync(user);
            }

            return res;
        }

        public virtual async Task GenerateEmailConfirmationEmailAsync(UrlHelper url, string id)
        {
            await GenerateEmailConfirmationEmailAsync(url, id, null);
        }

        /// <summary>
        /// Generates the email confirmation email.
        /// </summary>
        /// <param name="url">The URL helper from the controller asking for the email.</param>
        /// <param name="id">The id of the user who needs their email confirmed.</param>
        /// <returns>
        /// Generates a boiler-plate email including the confirmation token to send to
        /// the user.  Uses <see cref="ApplicationMessage"/> to generate both an HTML
        /// and Plain Text email when <see cref="EmailService"/> is used.
        /// </returns>
        public virtual async Task GenerateEmailConfirmationEmailAsync(UrlHelper url, string id, string temporaryPassword)
        {
            var token = await GenerateEmailConfirmationTokenAsync(id);
            var callbackUrl = url.Action("ConfirmEmail", "Account", new { userId = id, code = token }, "http");

            var html = "<html><body><p>Hi,</p><p>An account has been created for you on the <a href='http://sjatracker.elasticbeanstalk.com/'>SJA Tracker website</a>.</p>";
            html += $"<p>Please click <a href='{callbackUrl}'>this link</a> to confirm your email address before logging in.</p>";
            if (!string.IsNullOrEmpty(temporaryPassword))
            {
                html += "<p>This is your temporary password:</p>";
                html += $"<p><pre>{temporaryPassword}</pre></p>";
                html += "<p>You will need to change this after you have logged in.";
            }
            html += "<p>Please let me know if you have any problems.</p><p>Kind regards,</p><p>Tony Richards</p>";

            var text = "Hi,\n\nAn account has been created for you on http://sjatracker.elasticbeanstalk.com (the SJA Tracker website).\n\n";
            text += $"Please go to {callbackUrl} to confirm your email address before logging in.\n\n";
            if (!string.IsNullOrEmpty(temporaryPassword))
            {
                text += "This is your temporary password:\n\n";
                text += $"{temporaryPassword}\n\n";
                text += "You will need to change this after you have logged in.\n\n";
            }
            text += "Please let me know if you have any problems.\n\nKind regards,\n\nTony Richards";

            var msg = new ApplicationMessage
            {
                Body = text,
                HtmlBody = html,
                Destination = await GetEmailAsync(id),
                Subject = "SJA Tracker: Confirm your email address"
            };

            await EmailService.SendAsync(msg);
        }

        /// <summary>
        /// Generates the password reset email.
        /// </summary>
        /// <param name="url">The URL helper from the controller asking for the email.</param>
        /// <param name="id">The id of the user who needs their password resetting.</param>
        /// <returns>
        /// Generates a boiler-plate email including the confirmation token to send to
        /// the user.  Uses <see cref="ApplicationMessage"/> to generate both an HTML
        /// and Plain Text email when <see cref="EmailService"/> is used.
        /// </returns>
        public virtual async Task GeneratePasswordResetEmailAsync(UrlHelper url, string id)
        {
            var token = await GeneratePasswordResetTokenAsync(id);
            var callbackUrl = url.Action("ResetPassword", "Account", new { userId = id, code = token }, "http");

            var html = "<html><body><p>Hi,</p><p>You've asked to reset your password on the <a href='http://sjatracker.elasticbeanstalk.com/'>SJA Tracker website</a>.</p>";
            html += $"<p>Please click <a href='{callbackUrl}'>this link</a> to complete the reset.</p>";
            html += "<p>Please let me know if you have any problems.</p><p>Kind regards,</p><p>Tony Richards</p>";

            var text = "Hi,\n\nYou've asked to reset your password on http://sjatracker.elasticbeanstalk.com (the SJA Tracker website).\n\n";
            text += $"Please go to {callbackUrl} to complete the reset.\n\n";
            text += "Please let me know if you have any problems.\n\nKind regards,\n\nTony Richards";

            var msg = new ApplicationMessage
            {
                Body = text,
                HtmlBody = html,
                Destination = await GetEmailAsync(id),
                Subject = "SJA Tracker: Reset your password"
            };

            await EmailService.SendAsync(msg);
        }

        public override async Task<IdentityResult> SetEmailAsync(string userId, string email)
        {
            var res = await base.SetEmailAsync(userId, email);

            if (res.Succeeded)
            {
                var user = await Store.FindByIdAsync(userId);
                user.UserName = email;
                await Store.UpdateAsync(user);
            }

            return res;
        }
    }
}