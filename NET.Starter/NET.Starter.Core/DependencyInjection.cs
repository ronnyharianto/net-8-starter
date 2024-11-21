using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.Core.Services.Rfid;
using NET.Starter.Core.Services.Security;
using NET.Starter.Shared.Objects;
using NET.Starter.SDK;
using System.Reflection;

namespace NET.Starter.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.Configure<SecurityConfig>(options => configuration.Bind(nameof(SecurityConfig), options));

            services.AddScoped<TokenService>();
            services.AddScoped<AccountService>();
            services.AddScoped<RfidService>();

            services.RegisterRfidFixedReder();

            return services;
        }
    }
}
