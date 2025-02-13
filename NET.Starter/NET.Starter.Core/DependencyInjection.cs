using Microsoft.Extensions.DependencyInjection;
using NET.Starter.Core.Services.Security;
using System.Reflection;

namespace NET.Starter.Core
{
    /// <summary>
    /// Provides extension methods for registering dependencies in the Core layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers services and configurations for the Core layer into the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which services are added.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection RegisterCore(this IServiceCollection services)
        {
            // Register AutoMapper with assemblies from the Core layer
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register Services with scoped lifetimes
            services.AddScoped<TokenService>();

            return services;
        }
    }
}
