using AssignmentAPI.StartupExtensions.Seeders;
using System.Reflection;

namespace AssignmentAPI.StartupExtensions
{
    public static class SeedExtension
    {
        public static void RegisterSeeders(this IServiceCollection services)
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(p => !p.IsAbstract && !p.IsInterface && typeof(ISeeder).IsAssignableFrom(p));

            foreach (var type in types!)
            {
                services.AddScoped(typeof(ISeeder), type);
            }
        }

        public static async Task ApplySeedAsync(this IServiceScope scope)
        {
            var seeders = scope.ServiceProvider.GetServices<ISeeder>()
                .OrderBy(x => x.GetType().GetCustomAttributes<SeederAttribute>().FirstOrDefault()?.OrderId)
                .ThenBy(x => x.GetType().Name);

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}
