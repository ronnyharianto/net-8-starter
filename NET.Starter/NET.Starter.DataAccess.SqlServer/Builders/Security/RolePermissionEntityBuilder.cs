using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="RolePermission"/> entity.
    /// This class defines property configurations and seeds initial data for the RolePermission entity.
    /// </summary>
    public class RolePermissionEntityBuilder : EntityBaseBuilder<RolePermission>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="RolePermission"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            // Call the base configuration for the Permission entity.
            base.Configure(builder);

            builder
                .HasOne(e => e.Role)
                .WithMany(e => e.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Permission)
                .WithMany(e => e.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => new { e.RoleId, e.PermissionId })
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            SeedingDataAdministrator(builder);
        }

        /// <summary>
        /// Seeds initial data into the RolePermission table for Role Administrator.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{RolePermission}"/> used to configure the entity type.</param>
        private static void SeedingDataAdministrator(EntityTypeBuilder<RolePermission> builder)
        {
            // Adds predefined data for the general permission.
            builder.HasData(
                new RolePermission { Id = new Guid("971cf134-1f3e-4719-9d56-e60ed967a117"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"), Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );

            // Adds predefined data for the identity permission.
            builder.HasData(
                new RolePermission { Id = new Guid("02abe492-24b8-4a90-af18-3a282f9fcc85"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("69821b03-b132-4d35-88f0-5502908d50fa"), Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );
        }
    }
}
