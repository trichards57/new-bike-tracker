using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeTracker.Core.Models
{
    public class RefreshToken
    {
        public DateTimeOffset Expires { get; set; }
        public int Id { get; set; }
        public string TokenHash { get; set; }
        public string TokenSalt { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string UserId { get; set; }
    }
}
