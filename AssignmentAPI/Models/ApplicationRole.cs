using Microsoft.AspNetCore.Identity;

namespace AssignmentAPI.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole(string Name)
            : base(Name)
        { }

        public static ApplicationRole Admin = new ApplicationRole("Admin");
        public static ApplicationRole Visitor = new ApplicationRole("Visitor");
    }
}
