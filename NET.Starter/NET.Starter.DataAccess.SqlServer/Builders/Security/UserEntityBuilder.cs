using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Helpers;

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
                .HasMaxLength(20)
                .UseCollation(CollationConstants.SQL_Latin1_General_CP1_CS_AS); // Use this collation to make username case-sensitive when comparing

            builder
                .Property(e => e.EmailAddress)
                .HasMaxLength(100);

            builder
                .Property(e => e.Password)
                .HasConversion(v => CryptographyHelper.HashPassword(v), v => v)
                .HasMaxLength(2000);

            builder
                .Property(e => e.Fullname)
                .HasMaxLength(100);

            builder
                .HasIndex(e => e.Username)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            builder
                .HasIndex(e => e.EmailAddress)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();
        }
    }
}
