using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BikeTracker.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BikeTracker.Services
{
    /// <summary>
    /// Service to handle sending emails from the website
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// The standard From email address to use for all emails.
        /// </summary>
        private const string FromEmail = "Tony Richards <tony.richards@bath.edu>";

        /// <summary>
        /// Asynchronously sends the provided IdentityMessage.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <remarks>
        /// Sends the provided message using Amazon's Simple Email Service Client.
        ///
        /// If it is passed on ApplicationMessage, it will use it to populate
        /// both the HTML body and the plain text body.  Otherwise, it will only
        /// populate the message body and let Amazon SES work out how to format
        /// it properly.
        ///
        /// Requires environment variable AWS_ACCESS_KEY_ID.
        /// </remarks>
        public Task SendAsync(IdentityMessage message)
        {
            var subject = new Content(message.Subject);

            var applicationMessage = message as ApplicationMessage;

            var body = new Body { Text = new Content(message.Body) };
            if (applicationMessage != null)
                body.Html = new Content(applicationMessage.HtmlBody);

            var toEmail = new Destination(new List<string> { message.Destination });

            var email = new Message(subject, body);
            var request = new SendEmailRequest(FromEmail, toEmail, email);
            var region = Amazon.RegionEndpoint.EUWest1;

            var client = new AmazonSimpleEmailServiceClient(region);

            client.SendEmail(request);

            return Task.FromResult(0);
        }
    }
}