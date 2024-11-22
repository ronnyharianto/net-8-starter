using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.SDK.Implementations;
using NET.Starter.SDK.Managers;

namespace NET.Starter.SDK
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRfidFixedReder(this IServiceCollection services)
        {
            services.AddSingleton<RfidFixedReaderFactory>();

            services.AddSingleton<ZebraRfidReaderManager>();

            services.AddSingleton<MockRfidFixedReaderService>();
            services.AddSingleton<ZebraRfidFixedReaderService>();

            return services;
        }
    }
}
