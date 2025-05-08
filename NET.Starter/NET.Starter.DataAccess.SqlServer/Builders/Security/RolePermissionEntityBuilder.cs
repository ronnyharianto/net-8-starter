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
            // Adds predefined data for the identity permission.
            builder.HasData(
                new RolePermission { Id = new Guid("02abe492-24b8-4a90-af18-3a282f9fcc85"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("69821b03-b132-4d35-88f0-5502908d50fa"), Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );

            // Adds predefined data for the security permission.
            builder.HasData(
                // Permission
                new RolePermission { Id = new Guid("19dfa24e-a82a-449d-a1ce-11456ee5322c"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("a0d9d4d0-1b6f-4e4c-8e5e-a7c6a3b2abf1"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                
                // Role
                new RolePermission { Id = new Guid("2f0e1598-1ba3-495d-9ebe-673b15512c60"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("f324ee94-14d8-4fab-a703-dbe0cecfdf30"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("e298741f-3027-4299-bf56-66bd712219e0"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("58df83d3-2f40-450a-86d2-604dfa27fe35"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("a8b4b425-8827-4fde-b2c5-1fc3e059f061"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("223c7549-ddcc-4d05-95dc-9336c76a3e57"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("c28724cb-c0c1-462c-8a71-3161dc638920"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },

                // User
                new RolePermission { Id = new Guid("b6f275c7-1732-4289-90bc-30b75dfdb165"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("11183f54-13f3-4c4c-b484-bd170342ca70"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("6344bbaf-73cb-4f5e-8250-2f8130f062de"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("7d8b693b-a21f-4f7e-a0f9-b1f80a86e0bd"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("c3b9b0ba-9e9b-4db0-a66a-13412c77002f"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("f8568aa0-aae2-4ed0-9783-b75ff180ebdf"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("eeb6d346-4e4b-4273-b030-9a15218bf037"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("1bfb9a81-0f01-4981-b4d5-fce12f8b4d4d"), Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new RolePermission { Id = new Guid("c686e3ae-9a80-4474-872a-33340d19961f"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("3c390638-d417-421a-a99f-611591c07a4d"), Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );
        }
    }
}
