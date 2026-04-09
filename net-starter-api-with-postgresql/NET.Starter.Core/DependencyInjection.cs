using Microsoft.Extensions.DependencyInjection;
using NET.Starter.Core.Middlewares;
using NET.Starter.Core.Services.Organization;
using NET.Starter.Core.Services.Organization.Interfaces;
using NET.Starter.Core.Services.Security;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Core.Services.System;
using NET.Starter.DataAccess;
using System.Reflection;

namespace NET.Starter.Core
{
    /// <summary>
    /// Contains extension methods for registering core services and middlewares
    /// into the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the core services, middlewares, and AutoMapper profiles
        /// into the provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add registrations to.</param>
        /// <returns>The <see cref="IServiceCollection"/> with added services.</returns>
        public static IServiceCollection RegisterCore(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<AuthorizationFilter>();
                    options.Filters.Add<TransactionFilter<ApplicationDbContext>>();
                });

            services.AddAutoMapper(config => { }, Assembly.GetExecutingAssembly());

            #region Register Services with scoped lifetimes

            services.AddScoped<DocumentNumberingService>();
            services.AddScoped<FirebaseMessagingService>();

            services.AddScoped<TokenService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICompanyService, CompanyService>();

            #endregion            

            return services;
        }
    }
}
