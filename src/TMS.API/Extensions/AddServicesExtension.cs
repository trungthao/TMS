using Microsoft.Extensions.DependencyInjection;
using TMS.Domain.Services;
using TMS.Services;

namespace TMS.API.Extensions
{
    public static class AddServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IEmailService, EmailService>();
            return services;
        }
    }
}