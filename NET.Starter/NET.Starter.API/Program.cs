using NET.Starter.API.Extensions.StartupExtensions;
using NET.Starter.Core;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared;
using Serilog;

/// <summary>
/// Creates a new <see cref="WebApplicationBuilder"/> instance for application configuration.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configures the allowed origins for CORS based on the "CorsOrigin" setting in the configuration.
/// </summary>
var corsOrigin = builder.Configuration.GetValue<string>("CorsOrigin")?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();

/// <summary>
/// Registers shared services and configurations, such as logging or dependency injection.
/// </summary>
/// <param name="builder.Host">The hosting environment for the application.</param>
/// <param name="builder.Configuration">The application configuration.</param>
builder.Services.RegisterShared(builder.Host, builder.Configuration);

/// <summary>
/// Configures the data access layer, including database context and repositories.
/// </summary>
/// <param name="builder.Configuration">The application configuration.</param>
builder.Services.RegisterDataAccess(builder.Configuration);

/// <summary>
/// Registers core application services, such as business logic and domain services.
/// </summary>
builder.Services.RegisterCore();

/// <summary>
/// Configures the application to use controllers with global filters and custom serialization settings.
/// </summary>
builder.AddController();

/// <summary>
/// Adds and configures Swagger for API documentation, including JWT authentication support.
/// </summary>
builder.AddSwaggerGen();

/// <summary>
/// Configures Cross-Origin Resource Sharing (CORS) for allowed origins.
/// </summary>
/// <param name="corsOrigin">Array of allowed origins from the configuration.</param>
builder.AddCors(corsOrigin);

/// <summary>
/// Configures JWT authentication and authorization.
/// </summary>
builder.AddAuthentication();

/// <summary>
/// Builds the application pipeline for request handling.
/// </summary>
var app = builder.Build();

/// <summary>
/// Adds HTTP logging and Serilog for structured logging.
/// </summary>
app.UseHttpLogging().UseSerilogRequestLogging();

/// <summary>
/// Initializes the database context on application startup.
/// </summary>
app.UseDbContext();

if (app.Environment.IsDevelopment())
{
    /// <summary>
    /// Enables Swagger UI in development for API exploration.
    /// </summary>
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "NET.Starter.API";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

/// <summary>
/// Enables CORS and maps controller endpoints.
/// </summary>
app.UseCors();
app.MapControllers();

/// <summary>
/// Starts the web application and begins listening for incoming requests.
/// </summary>
app.Run();
