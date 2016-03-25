using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace BikeTracker.Controllers.Filters
{
    /// <summary>
    /// Action filter to enforce password expiry when accessing a page.
    /// </summary>
    /// This filter will allow an anonymous user to access the page.
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    [IgnoreCoverage]
    public class AllowAnonymousPasswordExpiresAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
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

    /// <summary>
    /// Action filter to enforce password expiry when accessing a page.
    /// </summary>
    /// This filter will not allow an anonymous user to access the page.  It can be used in place of the <seealso cref="System.Web.Mvc.AuthorizeAttribute"/>.
    /// <seealso cref="System.Web.Mvc.AuthorizeAttribute" />
    [IgnoreCoverage]
    public class AuthorizePasswordExpiresAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
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
}