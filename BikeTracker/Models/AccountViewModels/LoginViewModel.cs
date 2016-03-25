using BikeTracker.Controllers.Filters;
using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models.AccountViewModels
{
    /// <summary>
    /// Represents a request to log in to the system.
    /// </summary>
    [IgnoreCoverage]
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user should be remembered by the browser.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the user should be remembered; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}