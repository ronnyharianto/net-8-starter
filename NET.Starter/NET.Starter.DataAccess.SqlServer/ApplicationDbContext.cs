using Microsoft.EntityFrameworkCore;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Builders.Security;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Objects;

namespace NET.Starter.DataAccess.SqlServer
{
    /// <summary>
    /// Represents the application's database context, providing access to the database entities
    /// and configuring entity mappings for the application.
    /// </summary>
    public class ApplicationDbContext(DbContextOptions options, CurrentUserAccessor currentUserAccessor) : DbContextBase(options, currentUserAccessor)
    {
        #region Security

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        #endregion

        /// <summary>
        /// Configures the model relationships and mappings for the database entities.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity framework models.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base configuration from the parent class.
            base.OnModelCreating(modelBuilder);

            #region Security

            new PermissionEntityBuilder().Configure(modelBuilder.Entity<Permission>());
            new RoleEntityBuilder().Configure(modelBuilder.Entity<Role>());
            new RolePermissionEntityBuilder().Configure(modelBuilder.Entity<RolePermission>());
            new UserEntityBuilder().Configure(modelBuilder.Entity<User>());
            new UserRoleEntityBuilder().Configure(modelBuilder.Entity<UserRole>());

            #endregion
        }
    }
}
