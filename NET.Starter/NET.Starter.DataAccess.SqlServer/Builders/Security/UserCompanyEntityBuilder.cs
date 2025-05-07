using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="UserCompany"/> entity.
    /// This class defines property configurations and seeds initial data for the UserCompany entity.
    /// </summary>
    public class UserCompanyEntityBuilder : EntityBaseBuilder<UserCompany>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="UserCompany"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<UserCompany> builder)
        {
            // Call the base configuration for the UserCompany entity.
            base.Configure(builder);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.UserCompanies)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Company)
                .WithMany(e => e.UserCompanies)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => new { e.UserId, e.CompanyId })
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            builder
                .HasIndex(e => new { e.UserId })
                .HasFilter("[IsDefault] = 1")
                .IsUnique();
        }
    }
}
