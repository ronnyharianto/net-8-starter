using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NET.Starter.Shared.Helpers;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Configs;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace NET.Starter.Shared
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers shared services, including logging and scoped dependencies.
        /// </summary>
        /// <param name="services">Service collection to register dependencies.</param>
        /// <param name="host">Application host builder to configure logging.</param>
        /// <param name="configuration">Application configuration settings.</param>
        /// <returns>The modified service collection.</returns>
        public static IServiceCollection RegisterShared(this IServiceCollection services, IHostBuilder host, IConfiguration configuration)
        {
            #region Logging Configuration

            // Retrieve the Logging configuration settings from the configuration system.
            var loggingConfig = configuration.GetSection(nameof(LoggingConfig)).Get<LoggingConfig>();

            // Configure Serilog with different log levels for specific namespaces
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Fatal) // Suppress detailed logs from Entity Framework Core.
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning) // Suppress detailed logs from the ASP.NET Core routing middleware. Only log warning and errors (e.g., routing failures or misconfigurations) to avoid verbose request logs.
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning) // Suppress logs from the ASP.NET Core MVC framework. This hides logs such as 'Executing ActionResult' or 'Executing JsonResult' and only logs warnings and critical errors related to the MVC pipeline.
                .WriteTo.Console();

            // Check if the GrafanaLoki configuration is present
            if (loggingConfig?.GrafanaLoki != null)
            {
                // Add Grafana Loki sink
                loggerConfig.WriteTo.GrafanaLoki(loggingConfig.GrafanaLoki.EndpointUrl, loggingConfig.GrafanaLoki.LokiLabels);
            }

            // Assign configured logger to the global Log instance
            Log.Logger = loggerConfig.CreateLogger();

            // Integrate Serilog with the application's host
            host.UseSerilog();

            #endregion

            #region HTTP Logging Configuration

            // Enable HTTP request and response logging
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody | HttpLoggingFields.Duration;
            });

            Log.Logger.Information("HTTP logging is enabled.");

            #endregion

            #region Cryptography Initialization

            // Retrieve the RSA configuration settings from the configuration system.
            var rsaConfig = configuration.GetSection(nameof(RsaConfig)).Get<RsaConfig>();

            // Check if the RSA configuration is present
            if (rsaConfig != null)
            {
                // Initialize the CryptographyHelper with the retrieved RSA configuration.
                CryptographyHelper.Initialize(rsaConfig);

                Log.Logger.Information("RSA configuration is available. Cryptographic for RSA operations will be available.");
            }
            else
            {
                Log.Logger.Error("RSA configuration is missing. Cryptographic for RSA operations will not be available.");
            }

            #endregion

            #region Register Configuration Options

            // Register configuration options from appsettings.json or environment variables.
            // These configurations are bound using the Options Pattern and can be injected via IOptions<T>.

            // Security settings
            services.Configure<SecurityConfig>(opt => configuration.Bind(nameof(SecurityConfig), opt));

            #endregion

            #region Dependency Injection

            // Register 'CurrentUserAccessor' as a scoped service to store user identity per request lifecycle.
            services.AddScoped<CurrentUserAccessor>();

            // Retrieve the Http Client configuration settings from the configuration system.
            var httpClientConfig = configuration.GetSection(nameof(HttpClientConfig)).Get<HttpClientConfig>() ?? new();

            // Register 'HttpClientHelper' with custom configuration for HTTP requests.
            services.AddHttpClient<HttpClientHelper>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(httpClientConfig.Timeout); // Default timeout for requests.
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientConfig.HandlerLifetime)); // Lifetime of the message handler.

            #endregion

            return services;
        }
    }
}