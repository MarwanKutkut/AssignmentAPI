namespace AssignmentAPI.Models.Requests
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public decimal Price { get; set; }
    }
}
