using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Core.Models.AccountViewModels
{
    public class WhoAmIModel
    {
        public bool Authenticated { get; set; }
        public string RealName { get; set; }
        public IEnumerable<string> Role { get; set; }
        public string UserName { get; set; }
    }
}
