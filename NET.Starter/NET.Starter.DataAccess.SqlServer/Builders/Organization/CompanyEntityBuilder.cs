using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Organization;

namespace NET.Starter.DataAccess.SqlServer.Builders.Organization
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="Company"/> entity.
    /// This class defines property configurations and seeds initial data for the Company entity.
    /// </summary>
    public class CompanyEntityBuilder : EntityBaseBuilder<Company>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="Company"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            // Call the base configuration for the Company entity.
            base.Configure(builder);

            builder
                .Property(e => e.CompanyCode)
                .HasMaxLength(5);

            builder
                .Property(e => e.CompanyName)
                .HasMaxLength(100);

            builder
                .HasIndex(e => e.CompanyCode)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            builder
                .HasMany(e => e.Branches)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
