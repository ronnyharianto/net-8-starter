using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NET.Starter.DataAccess.Bases
{
    /// <summary>
    /// Configures entities that inherit from <see cref="EntityBase"/>.
    /// Applies a filter to exclude soft-deleted records.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    internal class EntityBaseBuilder<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Sets a global filter to ignore soft-deleted records.
        /// </summary>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .HasQueryFilter(e => e.RowStatus == 0);
        }
    }
}
