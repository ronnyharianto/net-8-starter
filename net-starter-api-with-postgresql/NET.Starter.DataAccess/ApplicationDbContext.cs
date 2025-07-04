using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Builders.Organization;
using NET.Starter.DataAccess.Builders.Security;
using NET.Starter.DataAccess.Builders.System;
using NET.Starter.DataAccess.Models.Organization;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.DataAccess.Models.System;
using NET.Starter.Shared.Objects;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET.Starter.Core")]

namespace NET.Starter.DataAccess
{
    /// <summary>
    /// The main database context for the application.
    /// <para>
    /// Inherits from <see cref="DbContextBase"/> which automatically manages auditing fields like Created/Modified.
    /// </para>
    /// <para>
    /// Contains <see cref="DbSet{T}"/> properties for system, security, and organization entities.
    /// Configures entity mappings via dedicated entity builders.
    /// Provides a method to seed initial example data.
    /// </para>
    /// </summary>
    internal class ApplicationDbContext(DbContextOptions _options, CurrentUserAccessor _currentUserAccessor, ILogger<ApplicationDbContext> _logger) 
        : DbContextBase(_options, _currentUserAccessor)
    {
        #region System

        internal virtual DbSet<DocumentNumbering> DocumentNumberings { get; set; }

        #endregion

        #region Security

        internal virtual DbSet<Permission> Permissions { get; set; }
        internal virtual DbSet<Role> Roles { get; set; }
        internal virtual DbSet<RolePermission> RolePermissions { get; set; }
        internal virtual DbSet<User> Users { get; set; }
        internal virtual DbSet<UserCompany> UserCompanies { get; set; }
        internal virtual DbSet<UserCompanyRole> UserCompanyRoles { get; set; }
        internal virtual DbSet<UserPushToken> UserPushTokens { get; set; }

        #endregion

        #region Organization

        internal virtual DbSet<Company> Companies { get; set; }

        #endregion

        /// <summary>
        /// Configures entity mappings using their respective builders.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region System

            new DocumentNumberingEntityBuilder().Configure(modelBuilder.Entity<DocumentNumbering>());

            #endregion

            #region Security

            new PermissionEntityBuilder().Configure(modelBuilder.Entity<Permission>());
            new RoleEntityBuilder().Configure(modelBuilder.Entity<Role>());
            new RolePermissionEntityBuilder().Configure(modelBuilder.Entity<RolePermission>());
            new UserEntityBuilder().Configure(modelBuilder.Entity<User>());
            new UserCompanyEntityBuilder().Configure(modelBuilder.Entity<UserCompany>());
            new UserCompanyRoleEntityBuilder().Configure(modelBuilder.Entity<UserCompanyRole>());
            new UserPushTokenEntityBuilder().Configure(modelBuilder.Entity<UserPushToken>());

            #endregion

            #region Organization

            new CompanyEntityBuilder().Configure(modelBuilder.Entity<Company>());

            #endregion
        }

        /// <summary>
        /// Seeds initial data for example companies and admin user if they don't exist.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task SeedDataUserAdminAsync()
        {
            #region Companies

            if (!Companies.Any(d => d.Code == "0001"))
            {
                Companies.Add(new Company
                {
                    Id = new Guid("5e5ca559-34be-42f2-ab42-90d6c66e889c"),
                    Code = "0001",
                    Name = "NET Starter Company",
                    Created = new DateTime(2025, 5, 7, 8, 51, 0)
                });

                _logger.LogInformation("Seeding data for example company 0001 added.");
            }

            if (!Companies.Any(d => d.Code == "0002"))
            {
                Companies.Add(new Company
                {
                    Id = new Guid("129ea8c9-9a01-4afa-84ed-85caca7d0a59"),
                    Code = "0002",
                    Name = "NET Boilerplate Company",
                    Created = new DateTime(2025, 5, 7, 8, 51, 0)
                });

                _logger.LogInformation("Seeding data for example company 0002 added.");
            }

            #endregion

            #region User Login

            if (!Users.IgnoreQueryFilters().Any(d => d.Username == "admin"))
            {
                Users.Add(new User
                {
                    Id = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"),
                    Username = "admin",
                    EmailAddress = "supersandre@gmail.com",
                    Password = "1234qwER",
                    Created = new DateTime(2025, 2, 12, 13, 30, 0, DateTimeKind.Utc)
                });

                _logger.LogInformation("Seeding data user admin added.");
            }

            #endregion

            await SaveChangesAsync();
        }
    }
}
