using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NET.Starter.API.Extensions.StartupExtensions;
using System.Text;
using Serilog;

namespace NET.Starter.API.Extensions.StartupExtensions
{
    /// <summary>
    /// Provides extension methods for configuring services in <see cref="WebApplicationBuilder"/>.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds and configures controllers for the application with custom JSON serialization settings.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <returns>The updated <see cref="WebApplicationBuilder"/> instance.</returns>
        public static WebApplicationBuilder AddController(this WebApplicationBuilder builder)
        {
            // Configure antiforgery settings to suppress the default X-Frame-Options header.
            builder.Services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            // Add controllers and configure Newtonsoft.Json to handle reference loops and nulls.
            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            Log.Logger.Information("Controllers and JSON settings configured.");
            return builder;
        }

        /// <summary>
        /// Adds and configures Swagger for API documentation with JWT Bearer authentication support.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <returns>The updated <see cref="WebApplicationBuilder"/> instance.</returns>
        public static WebApplicationBuilder AddSwaggerGen(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "NET.Starter.API", Version = "v1" });

                // Define the Bearer token scheme for Swagger UI
                c.AddSecurityDefinition("Bearer", new()
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
                });

                // Apply the Bearer token requirement globally
                c.AddSecurityRequirement(new()
                {
                    {
                        new()
                        {
                            Reference = new()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        []
                    }
                });

                // Enable support for Swagger annotations (e.g., [SwaggerOperation])
                c.EnableAnnotations();
            });

            return builder;
        }

        /// <summary>
        /// Configures Cross-Origin Resource Sharing (CORS) using specified allowed origins.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <param name="corsOrigin">An array of allowed origins. If null or empty, all origins are denied.</param>
        /// <returns>The updated <see cref="WebApplicationBuilder"/> instance.</returns>
        public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder, string[]? corsOrigin)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(cosrBuilder =>
                    cosrBuilder
                        .WithOrigins(corsOrigin ?? [])
                        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                        .WithHeaders("Content-Type", "Authorization", "X-Requested-With", "X-Platform", "Cache-Control")
                        .AllowCredentials()
                );
            });

            Log.Logger.Information("CORS configured for origins: {Origins}", string.Join(", ", corsOrigin ?? []));
            return builder;
        }

        /// <summary>
        /// Configures JWT-based authentication and authorization using settings from configuration.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <returns>The updated <see cref="WebApplicationBuilder"/> instance.</returns>
        public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
        {
            var authenticationConfig = builder.Configuration.GetSection("AuthenticationConfig");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authenticationConfig.GetValue<string>("JwtOption:Issuer"),
                        ValidAudience = authenticationConfig.GetValue<string>("JwtOption:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfig.GetValue<string>("JwtOption:SecretKey") ?? string.Empty)),
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            builder.Services.AddAuthorization();

            Log.Logger.Information("JWT Authentication and Authorization configured.");
            return builder;
        }

    }
}