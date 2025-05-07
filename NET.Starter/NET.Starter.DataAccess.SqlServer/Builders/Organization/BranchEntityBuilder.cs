using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Organization;

namespace NET.Starter.DataAccess.SqlServer.Builders.Organization
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="Branch"/> entity.
    /// This class defines property configurations and seeds initial data for the Branch entity.
    /// </summary>
    public class BranchEntityBuilder : EntityBaseBuilder<Branch>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="Branch"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<Branch> builder)
        {
            // Call the base configuration for the Branch entity.
            base.Configure(builder);

            builder
                .Property(e => e.BranchCode)
                .HasMaxLength(5);

            builder
                .Property(e => e.BranchName)
                .HasMaxLength(100);

            builder
                .HasIndex(e => e.BranchCode)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();
        }
    }
}
