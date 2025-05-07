using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NET.Starter.API.Extensions.StartupExtensions;
using Newtonsoft.Json;
using System.Text;

namespace NET.Starter.API.Extensions.StartupExtensions
{
    /// <summary>
    /// Extension methods for configuring services and middleware in the application pipeline.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the application to use controllers.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining further configurations.</returns>
        public static WebApplicationBuilder AddController(this WebApplicationBuilder builder)
        {
            // Configure antiforgery settings to suppress the default X-Frame-Options header.
            builder.Services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            builder.Services
                .AddControllers()
                // Configure JSON serialization to ignore reference loops.
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            return builder;
        }

        /// <summary>
        /// Configures the Swagger generator for API documentation, including support for JWT authentication.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining further configurations.</returns>
        public static WebApplicationBuilder AddSwaggerGen(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "NET.Starter.API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new()
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
                });

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

                c.EnableAnnotations();
            });

            return builder;
        }

        /// <summary>
        /// Configures Cross-Origin Resource Sharing (CORS) for the application, using the specified allowed origins.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <param name="corsOrigin">
        /// An array of allowed origins for CORS. If null or empty, no origins are explicitly allowed.
        /// </param>
        /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining further configurations.</returns>
        public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder, string[]? corsOrigin)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(cosrBuilder =>
                    cosrBuilder
                        .WithOrigins(corsOrigin ?? [])
                        .WithMethods("GET", "POST", "PUT", "DELETE")
                        .WithHeaders("Content-Type", "Authorization", "X-Requested-With")
                        .AllowCredentials()
                );
            });

            return builder;
        }

        /// <summary>
        /// Configures JWT authentication and authorization services for the application.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance being extended.</param>
        /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining further configurations.</returns>
        public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
        {
            var securityConfig = builder.Configuration.GetSection("SecurityConfig");

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
                        ValidIssuer = securityConfig.GetValue<string>("Issuer"),
                        ValidAudience = securityConfig.GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityConfig.GetValue<string>("SecretKey") ?? string.Empty)),
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            builder.Services.AddAuthorization();

            return builder;
        }

    }
}