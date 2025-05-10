namespace AssignmentAPI.Models.Requests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }
}
