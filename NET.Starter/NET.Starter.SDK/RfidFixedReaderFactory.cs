using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.SDK.Enums;
using NET.Starter.SDK.Implementations;
using NET.Starter.SDK.Interfaces;

namespace NET.Starter.SDK
{
    public class RfidFixedReaderFactory(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly bool _useMockRfidScanner = configuration.GetValue<bool>("UseMockRfidScanner");

        public IRfidFixedReader CreateRfidFixedReader(ReaderType readerType)
        {
            using var scope = _serviceProvider.CreateScope();

            if (_useMockRfidScanner) return scope.ServiceProvider.GetRequiredService<MockRfidFixedReaderService>();

            return readerType switch
            {
                ReaderType.Zebra => _serviceProvider.GetRequiredService<ZebraRfidFixedReaderService>(),
                _ => throw new ArgumentException("Merk tidak dikenali"),
            };
        }
    }
}
