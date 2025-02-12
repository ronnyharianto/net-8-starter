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

            SeedingData(builder);
        }

        /// <summary>
        /// Seeds initial data into the Permission table.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{Permission}"/> used to configure the entity type.</param>
        private static void SeedingData(EntityTypeBuilder<Permission> builder)
        {
            // Adds predefined data for the Permission table.
            builder.HasData(
                new Permission { Id = new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"), PermissionCode = PermissionConstants.MyPermission, Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );
        }
    }
}
