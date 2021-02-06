using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Domain.Constants;

namespace TMS.API.Extensions
{
    public static class AddConfigExtension
    {
        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Jwt));

            return services;
        }
    }
}
