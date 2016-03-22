using BikeTracker.Controllers.Filters;
using System.ComponentModel.DataAnnotations;

namespace BikeTracker.Models.AccountViewModels
{
    [IgnoreCoverage]
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}