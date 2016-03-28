using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.AccountViewModels
{
    /// <summary>
    /// Represents a request to reset a password.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the code provided in the password reset email.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the confirmation of the new password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        /// This must match <see cref="Password"/>.
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        /// <value>
        /// The email address of the user.
        /// </value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        /// This is required, it must match <see cref="ConfirmPassword"/>, and it must be between 6 and 100 characters long.
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}