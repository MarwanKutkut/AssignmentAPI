using AssignmentAPI.Models.Settings;

namespace AssignmentAPI.StartupExtensions
{
    public static class AppConfigExtensions
    {
        public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            return services;
        }
    }
}
