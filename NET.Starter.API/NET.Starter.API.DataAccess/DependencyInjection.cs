using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET.Starter.API")]

namespace NET.Starter.API.DataAccess
{
    internal static class DependencyInjection
    {
        internal static IServiceCollection RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(
                configuration.GetConnectionString("SqlServer"), (option) =>
                {
                    option.MigrationsAssembly("NET.Starter.API.DataAccess");
                    option.CommandTimeout(60);
                    option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));

            return services;
        }
    }
}
