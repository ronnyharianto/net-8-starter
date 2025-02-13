using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="Permission"/> entity.
    /// This class defines property configurations and seeds initial data for the Permission entity.
    /// </summary>
    public class PermissionEntityBuilder : EntityBaseBuilder<Permission>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="Permission"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Call the base configuration for the Permission entity.
            base.Configure(builder);

            builder
                .Property(e => e.PermissionCode)
                .HasMaxLength(50);

            builder
                .HasIndex(e => e.PermissionCode)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            SeedingData(builder);
        }

        /// <summary>
        /// Seeds initial data into the Permission table.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{Permission}"/> used to configure the entity type.</param>
        private static void SeedingData(EntityTypeBuilder<Permission> builder)
        {
            // Adds predefined data for the general permission.
            builder.HasData(
                new Permission { Id = new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"), PermissionCode = PermissionConstants.MyPermission, Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );

            // Adds predefined data for the identity permission.
            builder.HasData(
                new Permission { Id = new Guid("69821b03-b132-4d35-88f0-5502908d50fa"), PermissionCode = PermissionConstants.Identity.Admin, Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );

            // Adds predefined data for the security permission.
            builder.HasData(
                new Permission { Id = new Guid("a0d9d4d0-1b6f-4e4c-8e5e-a7c6a3b2abf1"), PermissionCode = PermissionConstants.Security.Permission.View, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"), PermissionCode = PermissionConstants.Security.Role.Menu, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("e298741f-3027-4299-bf56-66bd712219e0"), PermissionCode = PermissionConstants.Security.Role.View, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("a8b4b425-8827-4fde-b2c5-1fc3e059f061"), PermissionCode = PermissionConstants.Security.Role.Create, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"), PermissionCode = PermissionConstants.Security.Role.Update, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"), PermissionCode = PermissionConstants.Security.Role.Delete, Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );
        }
    }
}
