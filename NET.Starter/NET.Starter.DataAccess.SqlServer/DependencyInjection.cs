using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET.Starter.Shared.Objects.Configs;
using Serilog;
using System.Reflection;

namespace NET.Starter.DataAccess.SqlServer
{
    /// <summary>
    /// Provides an extension method for registering SQL Server data access dependencies.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the SQL Server database context and related settings into the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the dependencies will be added.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance used to retrieve the database connection string.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance for method chaining.</returns>
        /// <remarks>
        /// - This method retrieves database settings from the <see cref="SqlServerConfig"/> section in the configuration.
        /// - Configures the <see cref="ApplicationDbContext"/> with the following options:
        ///   - Uses SQL Server as the database provider.
        ///   - Connection string retrieved from the configuration system.
        ///   - Migrations are stored in the current executing assembly.
        ///   - Sets the command timeout for database operations based on the configuration.
        ///   - Enables <see cref="QuerySplittingBehavior.SplitQuery"/> for better performance when querying related data.
        /// - Logs an error if no connection string is found.
        /// </remarks>
        public static IServiceCollection RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve SQL Server configuration settings from the configuration system.
            var sqlServerSettings = configuration.GetSection(nameof(SqlServerConfig)).Get<SqlServerConfig>() ?? new();

            // Get the name of the current executing assembly for migration configuration.
            var executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            if (!string.IsNullOrWhiteSpace(sqlServerSettings.ConnectionString))
            {
                // Register ApplicationDbContext with SQL Server configuration.
                services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(
                    sqlServerSettings.ConnectionString, (option) =>
                    {
                        // Specify the assembly where migrations are located.
                        option.MigrationsAssembly(executingAssemblyName);

                        // Set the command timeout for database operations.
                        option.CommandTimeout(sqlServerSettings.CommandTimeout);

                        // Configure query splitting behavior to split queries for related data.
                        option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }));

                Log.Logger.Information("SQL Server already registered.");
            }
            else
            {
                Log.Logger.Error("No SQL Server connection string found in configuration.");
            }

            // Return the service collection for method chaining.
            return services;
        }
    }
}