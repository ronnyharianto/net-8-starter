using Microsoft.Extensions.DependencyInjection;
using NET.Starter.API.Shared.Objects;

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
