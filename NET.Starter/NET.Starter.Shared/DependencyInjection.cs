using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NET.Starter.Shared.Objects;
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
            #region Register Configuration Options

            // Register configuration options from appsettings.json or environment variables.
            // These configurations are bound using the Options Pattern and can be injected via IOptions<T>.

            // Security settings
            services.Configure<SecurityConfig>(opt => configuration.Bind(nameof(SecurityConfig), opt));

            #endregion

            #region Logging Configuration

            // Get LoggingConfig from dependency injection
            var loggingConfig = configuration.GetSection(nameof(LoggingConfig)).Get<LoggingConfig>();

            // Configure Serilog with different log levels for specific namespaces
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information() // Default minimum log level
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error) // Suppress detailed EF Core logs, only log errors to reduce noise in logs
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", LogEventLevel.Error) // Reduce logging noise from controller execution, log only errors
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Error) // Avoid logging every request routing step, only log errors
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", LogEventLevel.Error) // Suppress logging related to ObjectResult execution, unless there's an error
                .WriteTo.Console(); // Output logs to the console

            // Add Grafana Loki sink if configured
            if (loggingConfig?.GrafanaLoki != null)
            {
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

            #endregion

            #region Dependency Injection

            // Register 'CurrentUserAccessor' as a scoped service to store user identity per request lifecycle.
            services.AddScoped<CurrentUserAccessor>();

            #endregion

            return services;
        }
    }
}
