using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="UserFcmToken"/> entity.
    /// This class defines property configurations and seeds initial data for the UserFcmToken entity.
    /// </summary>
    public class UserFcmTokenEntityBuilder : EntityBaseBuilder<UserFcmToken>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="UserFcmToken"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<UserFcmToken> builder)
        {
            // Call the base configuration for the Permission entity.
            base.Configure(builder);

            builder
                .Property(e => e.FcmToken)
                .HasMaxLength(200);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.UserFcmTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => e.FcmToken)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();
        }
    }
}
