using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace NET.Starter.DataAccess
{
    /// <summary>
    /// Provides extension methods for initializing the database during application startup.
    /// </summary>
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Applies pending migrations, ensures database creation, and seeds initial admin user data when in development environment.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to extend.</param>
        /// <returns>The updated <see cref="WebApplication"/> instance.</returns>
        public async static Task<WebApplication> UseDbContext(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();

            Log.Logger.Information("Database migration completed.");

            if (app.Environment.IsDevelopment())
            {
                await dbContext.SeedDataUserAdminAsync();
            }

            return app;
        }
    }
}