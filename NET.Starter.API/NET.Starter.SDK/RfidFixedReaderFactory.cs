using Microsoft.Extensions.DependencyInjection;
using NET.Starter.SDK.Implementations;
using NET.Starter.SDK.Interfaces;

namespace NET.Starter.SDK
{
    public class RfidFixedReaderFactory(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public IRfidFixedReader CreateRfidFixedReader(string readerType)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            return readerType switch
            {
                "Zebra" => _serviceProvider.GetRequiredService<ZebraRfidFixedReader>(),
                "Mock" => _serviceProvider.GetRequiredService<MockRfidFixedReader>(),
                _ => throw new ArgumentException("Merk tidak dikenali"),
            };
        }
    }
}
