using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models.AccountViewModels
{
    public class UserAdminViewModel
    {
        public string Id { get; set; }

        [ReadOnly(true), Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required, EmailAddress, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [ReadOnly(true)]
        public string Role { get; set; }

        [ReadOnly(true)]
        public string RoleDisplayName { get; set; }

        public string RoleId { get; set; }
    }
}