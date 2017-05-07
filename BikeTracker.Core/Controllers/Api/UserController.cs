using BikeTracker.Core.Models.AdminViewModels;
using BikeTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeTracker.Core.Models;
using BikeTracker.Core.Models.AccountViewModels;
using BikeTracker.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeTracker.Core.Controllers.Api
{
    [Route("api/[controller]"), Authorize]
    public class UserController : Controller
    {
        private readonly IAdminService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IAdminService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<UserSummary>> GetAsync()
        {
            var users = _service.GetAll();

            var result = new List<UserSummary>();

            foreach (var u in users)
            {
                var summary = new UserSummary
                {
                    EmailAddress = u.Email,
                    Id = u.Id,
                    RealName = u.RealName,
                    Role = await _userManager.IsInRoleAsync(u, RoleStrings.AdminRole) ? "Administrator" : "Normal User"
                };
                result.Add(summary);
            }

            return result;
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
    }
}
