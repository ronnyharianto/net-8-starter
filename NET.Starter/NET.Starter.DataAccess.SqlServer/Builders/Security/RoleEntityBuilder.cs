using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="Role"/> entity.
    /// This class defines property configurations and seeds initial data for the Role entity.
    /// </summary>
    public class RoleEntityBuilder : EntityBaseBuilder<Role>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="Role"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            // Call the base configuration for the Role entity.
            base.Configure(builder);

            builder
                .Property(e => e.RoleCode)
                .HasMaxLength(50);

            builder
                .HasIndex(e => e.RoleCode)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            SeedingData(builder);
        }

        /// <summary>
        /// Seeds initial data into the Role table.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{Role}"/> used to configure the entity type.</param>
        private static void SeedingData(EntityTypeBuilder<Role> builder)
        {
            // Adds predefined data for the Role table.
            builder.HasData(
                new Role { Id = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), RoleCode = "Administrator", Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );
        }
    }
}
