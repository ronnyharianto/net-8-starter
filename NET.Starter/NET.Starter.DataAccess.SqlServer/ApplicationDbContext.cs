using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Builders.Security;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects;

namespace NET.Starter.DataAccess.SqlServer
{
    /// <summary>
    /// Represents the application's database context, providing access to the database entities
    /// and configuring entity mappings for the application.
    /// </summary>
    public class ApplicationDbContext(DbContextOptions options, CurrentUserAccessor currentUserAccessor, ILogger<ApplicationDbContext> logger) : DbContextBase(options, currentUserAccessor)
    {
        private readonly ILogger<ApplicationDbContext> _logger = logger;

        #region Security

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserFcmToken> UserFcmTokens { get; set; }

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
            new UserRoleEntityBuilder().Configure(modelBuilder.Entity<UserRole>());
            new UserFcmTokenEntityBuilder().Configure(modelBuilder.Entity<UserFcmToken>());

            #endregion
        }

        /// <summary>
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

                UserRoles.Add(new UserRole { 
                    Id = new Guid("8595575f-0851-47b5-8950-7583a8f28927"), 
                    UserId = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"), 
                    RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), 
                    Created = new DateTime(2025, 2, 12, 13, 30, 00) 
                });

                _logger.LogInformation("Seeding data user admin completed.");

                await SaveChangesAsync();
            }
        }
    }
}
