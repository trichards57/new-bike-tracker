using BikeTracker.Controllers.Filters;
using Microsoft.AspNet.Identity;

namespace BikeTracker.Models.IdentityModels
{
    [IgnoreCoverage]
    public class ApplicationMessage : IdentityMessage
    {
        public string HtmlBody { get; set; }
    }
}