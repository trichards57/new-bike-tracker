using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BikeTracker.Models
{
    public class UserAdminModel
    {
        public string Id { get; set; }

        [ReadOnly(true), Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required, EmailAddress, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [ReadOnly(true)]
        public string Role { get; set; }

        public string RoleId { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}