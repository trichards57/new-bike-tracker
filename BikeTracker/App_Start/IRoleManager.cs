using BikeTracker.Models.IdentityModels;
using System.Threading.Tasks;

namespace BikeTracker
{
    public interface IRoleManager
    {
        Task<ApplicationRole> FindByNameAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
