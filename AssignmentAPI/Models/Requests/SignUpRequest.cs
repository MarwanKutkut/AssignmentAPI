namespace AssignmentAPI.Models.Requests
{
    public class SignUpRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public UserRegistrationRequest GetUserRegistrationRequest()
        {
            return new UserRegistrationRequest()
            {
                FullName = FullName,
                Email = Email,
                UserName = UserName,
                Password = Password,
            };
        }
    }
}
