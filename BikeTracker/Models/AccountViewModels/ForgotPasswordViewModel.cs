using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.AccountViewModels
{
    /// <summary>
    /// Represents a request to reset a forgotten password.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        /// This value is required and must be a valid email address.
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}