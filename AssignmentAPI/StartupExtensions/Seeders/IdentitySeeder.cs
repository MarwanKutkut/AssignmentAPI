using AssignmentAPI.IdentityManagers;
using AssignmentAPI.Models;
using AssignmentAPI.Models.Requests;

namespace AssignmentAPI.StartupExtensions.Seeders
{
    [Seeder(Order.First)]
    public class IdentitySeeder(RoleManager roleManager, UserManager userManager): ISeeder
    {
        public async Task SeedAsync()
        {
            var roles = new[]
            {
                nameof(ApplicationRole.Admin),
                nameof(ApplicationRole.Visitor)
            };

            UserRegistrationRequest[] users =
                [
                    new()
                    {
                        Email = "admin@gmail.com",
                        FullName = "Admin",
                        UserName ="admin",
                        Password = "Admin@123",
                        Role = nameof(ApplicationRole.Admin),
                    },
                    new()
                    {
                        Email = "marwan@gmail.com",
                        FullName = "Marwan Kutkut",
                        UserName ="MarwanKutkut",
                        Password = "Marwan@123",
                    },
                ];

            await roleManager.AddRoles(roles);

            await userManager.AddUsersAsync(users);
        }
    }
}
