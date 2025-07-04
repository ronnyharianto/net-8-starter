using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NET.Starter.Shared.Helpers;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Configs;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.GoogleCloudLogging;
using Serilog.Sinks.Grafana.Loki;

namespace NET.Starter.Shared
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers shared services including logging, Google Cloud integration,
        /// HTTP logging, cryptography, timezone, and essential scoped services.
        /// </summary>
        /// <param name="services">Service collection to register dependencies into.</param>
        /// <param name="host">Host builder for configuring logging.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection RegisterShared(this IServiceCollection services, IHostBuilder host, IConfiguration configuration)
        {
            #region Logging Configuration

            var loggingConfig = configuration.GetSection(nameof(LoggingConfig)).Get<LoggingConfig>();
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Fatal) // Suppress detailed logs from Entity Framework Core.
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning) // Suppress detailed logs from the ASP.NET Core routing middleware. Only log warning and errors (e.g., routing failures or misconfigurations) to avoid verbose request logs.
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning) // Suppress logs from the ASP.NET Core MVC framework. This hides logs such as 'Executing ActionResult' or 'Executing JsonResult' and only logs warnings and critical errors related to the MVC pipeline.
                .WriteTo.Console();

            if (loggingConfig?.GrafanaLoki != null)
            {
                loggerConfig.WriteTo.GrafanaLoki(loggingConfig.GrafanaLoki.EndpointUrl, loggingConfig.GrafanaLoki.LokiLabels);
            }

            if (loggingConfig?.GoogleMonitoring != null)
            {
                loggerConfig.WriteTo.GoogleCloudLogging(new GoogleCloudLoggingSinkOptions
                {
                    ProjectId = loggingConfig.GoogleMonitoring.ProjectId,
                    LogName = loggingConfig.GoogleMonitoring.LogName,
                    UseSourceContextAsLogName = false,
                    GoogleCredentialJson = JsonConvertHelper.SerializeObject(loggingConfig.GoogleMonitoring.ServiceAccount),
                });
            }

            Log.Logger = loggerConfig.CreateLogger();
            host.UseSerilog();

            #endregion

            #region Storage Configuration

            var storageConfig = configuration.GetSection(nameof(StorageConfig)).Get<StorageConfig>();
            if (storageConfig?.GoogleCloudStorage != null)
            {
                GoogleCloudStorageHelper.Initialize(storageConfig.GoogleCloudStorage);
            }
            else
            {
                Log.Logger.Error("Google Cloud Storage configuration is missing. File storage operations to Google Cloud Storage will not be available.");
            }

            #endregion

            #region Messaging Configuration

            var messagingConfig = configuration.GetSection(nameof(MessagingConfig)).Get<MessagingConfig>();
            if (messagingConfig?.FirebaseMessaging != null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJsonParameters(new()
                    {
                        Type = messagingConfig.FirebaseMessaging.ServiceAccount.Type,
                        ProjectId = messagingConfig.FirebaseMessaging.ProjectId,
                        PrivateKey = messagingConfig.FirebaseMessaging.ServiceAccount.PrivateKey,
                        ClientEmail = messagingConfig.FirebaseMessaging.ServiceAccount.ClientEmail
                    })
                });

                Log.Logger.Information("Firebase Cloud Messaging configuration found; Firebase Messaging enabled.");
            }
            else
            {
                Log.Logger.Error("Firebase Cloud Messaging configuration is missing. Firebase Cloud Messaging operations will not be available.");
            }

            #endregion

            #region HTTP Logging Configuration

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody | HttpLoggingFields.Duration;
            });
            Log.Logger.Information("HTTP request and response logging enabled.");

            #endregion

            #region Cryptography Initialization

            var rsaConfig = configuration.GetSection(nameof(RsaConfig)).Get<RsaConfig>();
            if (rsaConfig != null)
            {
                CryptographyHelper.InitializeRsa(rsaConfig);
            }
            else
            {
                Log.Logger.Error("RSA configuration is missing. Cryptographic for RSA operations will not be available.");
            }

            #endregion

            #region Timezone Initialization

            var timeZoneConfig = configuration.GetSection(nameof(TimeZoneConfig)).Get<TimeZoneConfig>();
            if (timeZoneConfig != null)
            {
                TimeZoneHelper.Initialize(timeZoneConfig);
            }
            else
            {
                Log.Logger.Error("Timezone configuration is missing. Timezone operations will not be available.");
            }

            #endregion

            #region Register Configuration Options

            // Using Options Pattern to register configuration sections for DI injection.
            services.Configure<AuthenticationConfig>(opt => configuration.Bind(nameof(AuthenticationConfig), opt));
            services.Configure<TimeZoneConfig>(opt => configuration.Bind(nameof(TimeZoneConfig), opt));
            #endregion

            services.AddScoped<CurrentUserAccessor>();

            var httpClientConfig = configuration.GetSection(nameof(HttpClientConfig)).Get<HttpClientConfig>() ?? new();
            services.AddHttpClient<HttpClientHelper>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(httpClientConfig.Timeout);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientConfig.HandlerLifetime));

            return services;
        }
    }
}