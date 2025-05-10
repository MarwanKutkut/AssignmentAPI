namespace AssignmentAPI.Models.Requests
{
    public class CreateProductRequest
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public decimal Price { get; set; }
        public Product ToProduct()
        {
            return new Product
            {
                ArabicName = ArabicName,
                EnglishName = EnglishName,
                Price = Price,
            };
        }
    }
}
