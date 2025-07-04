using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.Shared.Objects.Configs;
using Serilog;
using System.Reflection;

namespace NET.Starter.DataAccess
{
    /// <summary>
    /// Provides extension methods to register DataAccess services into the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the database context <see cref="ApplicationDbContext"/> with the dependency injection container,
        /// configured based on the connection string and settings from <see cref="DatabaseConfig"/> section in the app configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configuration">The application configuration to read database settings from.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance for chaining.</returns>
        public static IServiceCollection RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>() ?? new();
            var executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            if (!string.IsNullOrWhiteSpace(databaseConfig.ConnectionString))
            {
                services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(
                    databaseConfig.ConnectionString, (option) =>
                    {
                        option.MigrationsAssembly(executingAssemblyName);
                        option.CommandTimeout(databaseConfig.CommandTimeout);
                        option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }));

                Log.Logger.Information("Database already registered.");
            }
            else
            {
                Log.Logger.Error("No Database connection string found in configuration.");
            }

            return services;
        }
    }
}