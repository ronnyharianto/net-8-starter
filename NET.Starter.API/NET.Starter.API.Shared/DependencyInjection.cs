using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NET.Starter.API.Shared.Objects;
using Serilog;

namespace NET.Starter.API.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterShared(this IServiceCollection services, IHostBuilder host, IConfiguration configuration)
        {
            var loggerConfig = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    .WriteTo.Console();

            Log.Logger = loggerConfig.CreateLogger();

            host.UseSerilog();

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody | HttpLoggingFields.Duration;
            });

            services.AddScoped<CurrentUserAccessor>();

            return services;
        }
    }
}
