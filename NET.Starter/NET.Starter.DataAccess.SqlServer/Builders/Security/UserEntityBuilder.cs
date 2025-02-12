using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="User"/> entity.
    /// This class defines property configurations and seeds initial data for the User entity.
    /// </summary>
    public class UserEntityBuilder : EntityBaseBuilder<User>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="User"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            // Call the base configuration for the Permission entity.
            base.Configure(builder);

            builder
                .Property(e => e.Username)
                .HasMaxLength(20);

            builder
                .Property(e => e.EmailAddress)
                .HasMaxLength(100);

            builder
                .Property(e => e.Password) //TODO: This should be encrypted
                .HasMaxLength(2000);

            builder
                .Property(e => e.Fullname)
                .HasMaxLength(100);

            SeedingData(builder);
        }

        /// <summary>
        /// Seeds initial data into the User table.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{User}"/> used to configure the entity type.</param>
        private static void SeedingData(EntityTypeBuilder<User> builder)
        {
            // Adds predefined data for the User table.
            builder.HasData(
                new User { Id = new Guid("73b4c7d1-e6a3-41dc-a8da-6d9a45092761"), Username = "admin", EmailAddress = "admin@example.com", Password="12345678", Fullname = "Administrator", Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );
        }
    }
}
