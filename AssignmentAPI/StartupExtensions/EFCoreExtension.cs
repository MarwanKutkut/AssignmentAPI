using AssignmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AssignmentAPI.StartupExtensions
{
    public static class EFCoreExtension
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("Default")));

            services.RegisterSeeders();

            return services;
        }

        public static async Task ApplyMigrationAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //dbcontext.Database.EnsureDeleted();

            if (dbcontext.Database.EnsureCreated())
            {
                dbcontext.Database.Migrate();

                await scope.ApplySeedAsync();
            }
        }
    }
}
