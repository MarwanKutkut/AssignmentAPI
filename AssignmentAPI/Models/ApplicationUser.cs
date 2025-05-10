using Microsoft.AspNetCore.Identity;

namespace AssignmentAPI.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
