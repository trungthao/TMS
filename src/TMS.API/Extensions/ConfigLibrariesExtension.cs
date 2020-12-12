using Microsoft.Extensions.DependencyInjection;
using TMS.Library;
using TMS.Library.Interfaces;

namespace TMS.API.Extensions
{
    public static class ConfigLibrariesExtension
    {
        public static IServiceCollection AddLibraries(this IServiceCollection services)
        {
            services.AddSingleton<IConfigService, ConfigService>();
            services.AddScoped<IDatabaseService, DatabaseService>();

            return services;
        }
    }
}