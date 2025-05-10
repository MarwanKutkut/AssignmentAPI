namespace AssignmentAPI.Models.Requests
{
    public class UserRegistrationRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = nameof(ApplicationRole.Visitor);
    }
}
