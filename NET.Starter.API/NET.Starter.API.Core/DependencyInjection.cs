using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.API.Core.Services.Security;
using NET.Starter.API.Shared.Objects;
using System.Reflection;

namespace NET.Starter.API.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.Configure<SecurityConfig>(options => configuration.Bind(nameof(SecurityConfig), options));

            services.AddScoped<TokenService>();
            services.AddScoped<AccountService>();

            return services;
        }
    }
}
