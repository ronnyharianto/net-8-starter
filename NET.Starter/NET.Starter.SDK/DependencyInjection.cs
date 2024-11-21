using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.SDK.Implementations;

namespace NET.Starter.SDK
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRfidFixedReder(this IServiceCollection services)
        {
            services.AddSingleton<MockRfidFixedReader>();
            services.AddSingleton<ZebraRfidFixedReader>();

            services.AddSingleton<RfidFixedReaderFactory>();

            return services;
        }
    }
}
