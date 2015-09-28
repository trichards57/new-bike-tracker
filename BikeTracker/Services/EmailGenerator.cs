using BikeTracker.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BikeTracker.Services
{
    public static class EmailGenerator
    {
        public async static Task GenerateEmailConfirmationEmailAsync(this ApplicationUserManager userManager, UrlHelper url, string id)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(id);
            var callbackUrl = url.Action("ConfirmEmail", "Account", new { userId = id, code = token }, "http");

            var html = "<html><body><p>Hi,</p><p>An account has been created for you on the <a href='http://sjatracker.elasticbeanstalk.com/'>SJA Tracker website</a>.</p>";
            html += $"<p>Please click <a href='{callbackUrl}'>this link</a> to confirm your email address before logging in.</p>";
            html += "<p>Please let me know if you have any problems.</p><p>Kind regards,</p><p>Tony Richards</p>";

            var text = "Hi,\n\nAn account has been created for you on http://sjatracker.elasticbeanstalk.com (the SJA Tracker website).\n\n";
            text += $"Please go to {callbackUrl} to confirm your email address before logging in.\n\n";
            text += "Please let me know if you have any problems.\n\nKind regards,\n\nTony Richards";

            var msg = new ApplicationMessage
            {
                Body = text,
                HtmlBody = html,
                Destination = await userManager.GetEmailAsync(id),
                Subject = "SJA Tracker: Confirm your email address"
            };

            await userManager.EmailService.SendAsync(msg);
        }
    }
}