using BikeTracker.Core.Data;
using BikeTracker.Core.Models;
using BikeTracker.Core.Services.Interfaces;
using System.Collections.Generic;

namespace BikeTracker.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }
    }
}
