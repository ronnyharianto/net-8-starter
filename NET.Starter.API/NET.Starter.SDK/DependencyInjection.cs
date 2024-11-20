using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.SDK.Implementations;

namespace NET.Starter.SDK
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRfidFixedReder(this IServiceCollection services)
        {
            services.AddScoped<MockRfidFixedReader>();
            services.AddScoped<ZebraRfidFixedReader>();

            services.AddScoped<RfidFixedReaderFactory>();

            return services;
        }
    }
}
