using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="UserRole"/> entity.
    /// This class defines property configurations and seeds initial data for the UserRole entity.
    /// </summary>
    public class UserRoleEntityBuilder : EntityBaseBuilder<UserRole>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="UserRole"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Call the base configuration for the Permission entity.
            base.Configure(builder);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => new { e.UserId, e.RoleId })
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            SeedingData(builder);
        }

        /// <summary>
        /// Seeds initial data into the UserRole table.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{UserRole}"/> used to configure the entity type.</param>
        private static void SeedingData(EntityTypeBuilder<UserRole> builder)
        {
            // Adds predefined data for the UserRole table.
            builder.HasData(
                new UserRole { Id = new Guid("8595575f-0851-47b5-8950-7583a8f28927"), UserId = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );
        }
    }
}
