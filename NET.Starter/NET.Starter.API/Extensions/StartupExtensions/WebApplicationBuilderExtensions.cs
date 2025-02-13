using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NET.Starter.API.Extensions.StartupExtensions;
using NET.Starter.API.Middlewares;
using NET.Starter.DataAccess.SqlServer;
using System.Text;

namespace NET.Starter.API.Extensions.StartupExtensions
{
    /// <summary>
    /// Extension methods for configuring services and middleware in the application pipeline.
    /// </summary>
    /// <remarks>
    /// This class provides helper methods to simplify common configuration tasks such as 
    /// setting up controllers, authentication, Swagger, and CORS policies. These methods 
    /// extend the functionality of <see cref="WebApplicationBuilder"/> to streamline setup 
    /// for ASP.NET Core applications.
    /// </remarks>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the application to use controllers with antiforgery settings and global filters.
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

            // Add controllers and apply global filters.
            builder.Services
                .AddControllers(options =>
                {
                    // Add custom filters for authorization and transactions.
                    options.Filters.Add<AuthorizationFilter>();
                    options.Filters.Add<TransactionFilter<ApplicationDbContext>>();
                })
                // Configure JSON serialization to ignore reference loops.
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

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
                // Configure basic API metadata.
                c.SwaggerDoc("v1", new() { Title = "NET.Starter.API", Version = "v1" });

                // Add a JWT Bearer authentication definition for the Swagger UI.
                c.AddSecurityDefinition("Bearer", new()
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
                });

                // Define security requirements for endpoints.
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

                // Enable annotation support in Swagger.
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
                // Configure CORS to allow specific methods, headers, and credentials.
                options.AddDefaultPolicy(cosrBuilder =>
                    cosrBuilder
                        .AllowAnyMethod()
                        .WithOrigins(corsOrigin ?? [])
                        .AllowAnyHeader()
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
            // Retrieve security configuration settings.
            var securityConfig = builder.Configuration.GetSection("SecurityConfig");

            builder.Services
                .AddAuthentication(options =>
                {
                    // Set the default authentication and challenge schemes to JWT Bearer.
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // Configure token validation parameters.
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = securityConfig.GetValue<string>("Issuer"),
                        ValidAudience = securityConfig.GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityConfig.GetValue<string>("SecretKey") ?? string.Empty)),
                        ClockSkew = TimeSpan.Zero // Eliminate token lifetime tolerance.
                    };
                });

            // Add authorization services.
            builder.Services.AddAuthorization();

            return builder;
        }

    }
}