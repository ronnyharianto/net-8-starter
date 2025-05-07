using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Builders.Organization;
using NET.Starter.DataAccess.SqlServer.Builders.Security;
using NET.Starter.DataAccess.SqlServer.Models.Organization;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET.Starter.Core")]

namespace NET.Starter.DataAccess.SqlServer
{
    /// <summary>
    /// Represents the application's database context, providing access to the database entities
    /// and configuring entity mappings for the application.
    /// </summary>
    internal class ApplicationDbContext(DbContextOptions options, CurrentUserAccessor currentUserAccessor, ILogger<ApplicationDbContext> logger) : DbContextBase(options, currentUserAccessor)
    {
        private readonly ILogger<ApplicationDbContext> _logger = logger;

        #region Security

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserCompany> UserCompanies { get; set; }
        public virtual DbSet<UserCompanyRole> UserCompanyRoles { get; set; }

        #endregion

        #region Organization

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }

        #endregion

        /// <summary>
        /// Configures the model relationships and mappings for the database entities.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity framework models.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base configuration from the parent class.
            base.OnModelCreating(modelBuilder);

            // Configure the default collation for all string columns in the database.
            modelBuilder.UseCollation(CollationConstants.SQL_Latin1_General_CP1_CI_AS);

            #region Security

            new PermissionEntityBuilder().Configure(modelBuilder.Entity<Permission>());
            new RoleEntityBuilder().Configure(modelBuilder.Entity<Role>());
            new RolePermissionEntityBuilder().Configure(modelBuilder.Entity<RolePermission>());
            new UserEntityBuilder().Configure(modelBuilder.Entity<User>());
            new UserCompanyEntityBuilder().Configure(modelBuilder.Entity<UserCompany>());
            new UserCompanyRoleEntityBuilder().Configure(modelBuilder.Entity<UserCompanyRole>());

            #endregion

            #region Organization

            new CompanyEntityBuilder().Configure(modelBuilder.Entity<Company>());
            new BranchEntityBuilder().Configure(modelBuilder.Entity<Branch>());

            #endregion
        }

        /// <summary>
        /// Seeds the database with an example company and branch if it does not already exist.
        /// Seeds the database with an admin user and assigns the admin role if it does not already exist.
        /// </summary>
        /// <remarks>
        /// This method checks if a user with the username "admin" exists in the database.
        /// If not, it creates an admin user with predefined credentials and assigns them an admin role.
        /// The changes are then saved asynchronously.
        /// </remarks>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        internal async Task SeedDataUserAdminAsync()
        {
            #region Companies

            if (!Companies.Any(d => d.CompanyCode == "0001"))
            {
                Companies.Add(new Company
                {
                    Id = new Guid("5e5ca559-34be-42f2-ab42-90d6c66e889c"),
                    CompanyCode = "0001",
                    CompanyName = "NET Starter Company",
                    Created = new DateTime(2025, 5, 7, 8, 51, 0)
                });

                _logger.LogInformation("Seeding data for example company 0001 added.");
            }

            if (!Companies.Any(d => d.CompanyCode == "0002"))
            {
                Companies.Add(new Company
                {
                    Id = new Guid("129ea8c9-9a01-4afa-84ed-85caca7d0a59"),
                    CompanyCode = "0002",
                    CompanyName = "NET Boilerplate Company",
                    Created = new DateTime(2025, 5, 7, 8, 51, 0)
                });

                _logger.LogInformation("Seeding data for example company 0002 added.");
            }

            #endregion

            #region Branches

            if (!Branches.Any(d => d.BranchCode == "1001"))
            {
                Branches.Add(new Branch
                {
                    Id = new Guid("22165bd3-f9c9-410a-bce2-54b312df217d"),
                    CompanyId = new Guid("5e5ca559-34be-42f2-ab42-90d6c66e889c"),
                    BranchCode = "1001",
                    BranchName = "NET Starter Branch 1",
                    Created = new DateTime(2025, 5, 7, 8, 51, 0)
                });

                _logger.LogInformation("Seeding data for example branch 1001 added.");
            }

            if (!Branches.Any(d => d.BranchCode == "1002"))
            {
                Branches.Add(new Branch
                {
                    Id = new Guid("b5107f7f-1d98-4457-8810-f13df6ceed3a"),
                    CompanyId = new Guid("5e5ca559-34be-42f2-ab42-90d6c66e889c"),
                    BranchCode = "1002",
                    BranchName = "NET Starter Branch 2",
                    Created = new DateTime(2025, 5, 7, 8, 51, 0)
                });

                _logger.LogInformation("Seeding data for example branch 1002 added.");
            }

            if (!Branches.Any(d => d.BranchCode == "2001"))
            {
                Branches.Add(new Branch
                {
                    Id = new Guid("f25d598a-c349-4001-ac5b-1f8e4b11d483"),
                    CompanyId = new Guid("129ea8c9-9a01-4afa-84ed-85caca7d0a59"),
                    BranchCode = "2001",
                    BranchName = "NET Boilerplate Branch 1",
                    Created = new DateTime(2025, 5, 7, 8, 51, 0)
                });

                _logger.LogInformation("Seeding data for example branch 2001 added.");
            }

            #endregion



            if (!Users.Any(d => d.Username == "admin"))
            {
                Users.Add(new User
                {
                    Id = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"),
                    Username = "admin",
                    EmailAddress = "admin@example.com",
                    Password = "1234qwER",
                    Fullname = "Administrator",
                    Created = new DateTime(2025, 2, 12, 13, 30, 00)
                });

                UserCompanies.AddRange([
                    new UserCompany { Id = new Guid("b6a570c5-2ca5-4b21-bc12-76610405f594"), UserId = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"), CompanyId = new Guid("5e5ca559-34be-42f2-ab42-90d6c66e889c"), IsDefault = true, Created = new DateTime(2025, 5, 7, 8, 57, 00) },
                    new UserCompany { Id = new Guid("0aee6006-10e0-41cb-be16-976a901e5012"), UserId = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"), CompanyId = new Guid("129ea8c9-9a01-4afa-84ed-85caca7d0a59"), Created = new DateTime(2025, 5, 7, 8, 57, 00) }
                ]);

                UserCompanyRoles.Add(new UserCompanyRole { 
                    Id = new Guid("8595575f-0851-47b5-8950-7583a8f28927"), 
                    UserCompanyId = new Guid("b6a570c5-2ca5-4b21-bc12-76610405f594"), 
                    RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 
                    Created = new DateTime(2025, 5, 7, 8, 58, 00)
                });

                _logger.LogInformation("Seeding data user admin added.");
            }

            await SaveChangesAsync();
        }
    }
}
