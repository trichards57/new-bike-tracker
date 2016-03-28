using Microsoft.AspNet.Identity;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker.Models.IdentityModels
{
    /// <summary>
    /// Represents a message sent by the application to a user by means other than the website.
    /// </summary>
    /// <seealso cref="IdentityMessage" />
    [ExcludeFromCodeCoverage]
    public class ApplicationMessage : IdentityMessage
    {
        /// <summary>
        /// Gets or sets the HTML body for the message.
        /// </summary>
        /// <value>
        /// The HTML body.
        /// </value>
        public string HtmlBody { get; set; }
    }
}