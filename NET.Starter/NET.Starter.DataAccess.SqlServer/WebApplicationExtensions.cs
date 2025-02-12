using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace NET.Starter.DataAccess.SqlServer
{
    /// <summary>
    /// Provides extension methods for configuring database context in a <see cref="WebApplication"/>.
    /// </summary>
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Applies database migrations and ensures the database is created during application startup.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
        /// <returns>The same <see cref="WebApplication"/> instance for method chaining.</returns>
        /// <remarks>
        /// This method creates a scoped service provider to resolve the <see cref="ApplicationDbContext"/>,
        /// applies pending migrations, and ensures that the database is created if it does not exist.
        /// </remarks>
        public static WebApplication UseDbContext(this WebApplication app)
        {
            using var scope = app.Services.CreateScope(); // Create a scope for resolving services.
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Apply any pending database migrations.
            dbContext.Database.Migrate();

            // Ensure the database is created if it does not already exist.
            dbContext.Database.EnsureCreated();

            Log.Logger.Information("Database migration completed.");

            // Return the application instance for method chaining.
            return app;
        }
    }
}