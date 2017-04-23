using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BikeTracker.Core.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using BikeTracker.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeTracker.Core.Controllers.Api
{
    [Route("api/[controller]"), Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("whoami"), AllowAnonymous]
        public async Task<IActionResult> WhoAmI()
        {
            var res = new WhoAmIModel
            {
                Authenticated = User.Identity.IsAuthenticated
            };

            if (res.Authenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                res.RealName = user.RealName;
                res.Role = await _userManager.GetRolesAsync(user);
                res.UserName = user.UserName;
            }

            return Ok(res);
        }
    }
}
