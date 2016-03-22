using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace BikeTracker.Controllers
{
    public abstract class UserBaseController : Controller
    {
        private ISignInManager _signInManager;
        private IUserManager _userManager;

        public UserBaseController()
        {
        }

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