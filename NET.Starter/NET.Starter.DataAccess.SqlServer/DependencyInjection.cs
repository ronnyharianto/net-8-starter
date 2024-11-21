using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NET.Starter.DataAccess.SqlServer
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(
                configuration.GetConnectionString("SqlServer"), (option) =>
                {
                    option.MigrationsAssembly("NET.Starter.DataAccess.SqlServer");
                    option.CommandTimeout(60);
                    option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));

            return services;
        }
    }
}
