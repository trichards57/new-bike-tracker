using Microsoft.AspNet.Identity;

namespace BikeTracker.Models.IdentityModels
{
    public class ApplicationMessage : IdentityMessage
    {
        public string HtmlBody { get; set; }
    }
}