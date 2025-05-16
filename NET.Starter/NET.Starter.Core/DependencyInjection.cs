using Microsoft.Extensions.DependencyInjection;
using NET.Starter.Core.Middlewares;
using NET.Starter.Core.Services.Organization;
using NET.Starter.Core.Services.Security;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using System.Reflection;

namespace NET.Starter.Core
{
    /// <summary>
    /// Provides extension methods for registering dependencies in the Core layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers services and configurations for the Core layer into the dependency injection container.
        /// </summary>
        public static IServiceCollection RegisterCore(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<AuthorizationFilter>();
                    options.Filters.Add<TransactionFilter<ApplicationDbContext>>();
                });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Register Services with scoped lifetimes

            services.AddScoped<TokenService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IBranchService, BranchService>();

            #endregion            

            return services;
        }
    }
}
