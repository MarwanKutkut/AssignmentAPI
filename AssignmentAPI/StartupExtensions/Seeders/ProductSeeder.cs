using AssignmentAPI.Models;

namespace AssignmentAPI.StartupExtensions.Seeders
{
    [Seeder]
    public class ProductSeeder(AppDbContext db) : ISeeder
    {
        public async Task SeedAsync()
        {
            Product[] products =
                [
                    new Product
                    {
                        ArabicName = "تفاح",
                        EnglishName = "Apple",
                        Price = 0.8m,
                    },
                    new Product
                    {
                        ArabicName = "موز",
                        EnglishName = "Banana",
                        Price = 1.7m,
                    },
                    new Product
                    {
                        ArabicName = "بندورة",
                        EnglishName = "Tomato",
                        Price = 0.4m,
                    },
                ];

            db.Products.AddRange(products);
            await db.SaveChangesAsync();
        }
    }
}
