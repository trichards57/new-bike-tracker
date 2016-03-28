using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.AccountViewModels
{
    /// <summary>
    /// Represents the data provided to a user administrator.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserAdminViewModel
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [Required, EmailAddress, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        [ReadOnly(true)]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the display name of the role.
        /// </summary>
        /// <value>
        /// The display name of the role.
        /// </value>
        [ReadOnly(true)]
        public string RoleDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the role.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [ReadOnly(true), Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}