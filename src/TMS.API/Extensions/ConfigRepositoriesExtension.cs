using Microsoft.Extensions.DependencyInjection;
using TMS.Domain.Repositories;
using TMS.Repositories;

namespace TMS.API.Extensions
{
    public static class ConfigRepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}