using FluentValidation;
using System.Reflection;

namespace AssignmentAPI.StartupExtensions
{
    public static class ValidatorsExtension
    {
        public static IServiceCollection InjectValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
