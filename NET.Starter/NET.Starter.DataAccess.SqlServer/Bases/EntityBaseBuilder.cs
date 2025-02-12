using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NET.Starter.DataAccess.SqlServer.Bases
{
    /// <summary>
    /// Base class for configuring entity mappings to the database.
    /// Provides default configurations for all entities inheriting from <see cref="EntityBase"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being configured, which must inherit from <see cref="EntityBase"/>.</typeparam>
    public class EntityBaseBuilder<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Configures the entity's mappings to the database.
        /// Adds a global query filter to exclude soft-deleted records (where <see cref="EntityBase.RowStatus"/> is not 0).
        /// </summary>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity.</param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Add a global query filter to automatically filter out soft-deleted records (RowStatus != 0).
            builder
                .HasQueryFilter(e => e.RowStatus == 0);
        }
    }
}
