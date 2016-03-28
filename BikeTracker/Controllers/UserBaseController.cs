using Microsoft.AspNet.Identity.Owin;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    /// <summary>
    /// The base class for controllers that manage users.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public abstract class UserBaseController : Controller
    {
        /// <summary>
        /// The ISignInManager used by the controller.
        /// </summary>
        private ISignInManager _signInManager;
        /// <summary>
        /// The IUserManager used by the controller.
        /// </summary>
        private IUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBaseController"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public UserBaseController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBaseController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        public UserBaseController(IUserManager userManager, ISignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// Gets the sign in manager.
        /// </summary>
        /// <value>
        /// The sign in manager.
        /// </value>
        public ISignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        public IUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}