using BikeTracker.Models.Contexts;
using BikeTracker.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Diagnostics.CodeAnalysis;

namespace BikeTracker
{
    [ExcludeFromCodeCoverage]
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IRoleManager
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store)
            : base(store)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleStore = new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>());
            return new ApplicationRoleManager(roleStore);
        }
    }
}