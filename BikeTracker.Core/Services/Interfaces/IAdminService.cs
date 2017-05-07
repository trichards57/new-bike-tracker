using System.Collections.Generic;
using BikeTracker.Core.Models;

namespace BikeTracker.Core.Services.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<ApplicationUser> GetAll();
    }
}
