using Microsoft.Extensions.DependencyInjection;
using NET.Starter.API.Shared.Objects;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET.Starter.API")]

namespace NET.Starter.API.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterShared(this IServiceCollection services)
        {
           services.AddScoped<CurrentUserAccessor>();

            return services;
        }
    }
}
