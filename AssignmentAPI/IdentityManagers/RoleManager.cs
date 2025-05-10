using AssignmentAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AssignmentAPI.IdentityManagers
{
    public class RoleManager : RoleManager<ApplicationRole>
    {
        public RoleManager(IRoleStore<ApplicationRole> store, IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {   
        }

        public async Task AddRoles(string[] roles)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));

            foreach (var role in roles)
            {
                if (!await RoleExistsAsync(role))
                {
                    await CreateAsync(new ApplicationRole(role));
                }
            }
        }
    }
}
