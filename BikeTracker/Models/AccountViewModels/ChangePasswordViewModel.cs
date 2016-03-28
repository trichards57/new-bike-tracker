using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.AccountViewModels
{
    /// <summary>
    /// Model to represent a request from the user to change their password.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets the password confirmation.
        /// </summary>
        /// <value>
        /// The password confirmation.
        /// </value>
        /// This value must match <see cref="NewPassword"/>.
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the requested new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        /// This value is required, must be at least 6 characters long, cannot be more than 100 characters long and 
        /// must match <see cref="ConfirmPassword"/>.
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the user's old password.
        /// </summary>
        /// <value>
        /// The old password.
        /// </value>
        /// This valus is required.
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }
    }
}