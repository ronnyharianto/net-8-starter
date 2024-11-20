using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.SDK.Implementations;
using NET.Starter.SDK.Interfaces;

namespace NET.Starter.SDK
{
    public class RfidFixedReaderFactory(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly bool _useMockRfidScanner = configuration.GetValue<bool>("UseMockRfidScanner");

        public IRfidFixedReader CreateRfidFixedReader(string readerType)
        {
            using var scope = _serviceProvider.CreateScope();

            if (_useMockRfidScanner) return scope.ServiceProvider.GetRequiredService<MockRfidFixedReader>();

            return readerType switch
            {
                "Zebra" => _serviceProvider.GetRequiredService<ZebraRfidFixedReader>(),
                _ => throw new ArgumentException("Merk tidak dikenali"),
            };
        }
    }
}
