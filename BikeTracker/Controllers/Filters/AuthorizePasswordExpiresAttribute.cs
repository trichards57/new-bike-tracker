using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace BikeTracker.Controllers.Filters
{
    [IgnoreCoverage]
    public class AuthorizePasswordExpiresAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                var appUser = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(filterContext.HttpContext.User.Identity.GetUserId()).Result;

                if (appUser.MustResetPassword)
                    filterContext.HttpContext.Response.RedirectToRoute(new { action = "ChangePassword", controller = "Manage" });
            }

            base.OnAuthorization(filterContext);
        }
    }

    [IgnoreCoverage]
    public class AllowAnonymousPasswordExpiresAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                var appUser = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(filterContext.HttpContext.User.Identity.GetUserId()).Result;

                if (appUser.MustResetPassword)
                    filterContext.HttpContext.Response.RedirectToRoute(new { action = "ChangePassword", controller = "Manage" });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}