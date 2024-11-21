using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NET.Starter.DataAccess.SqlServer.Bases
{
    public class EntityBaseBuilder<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .HasQueryFilter(e => e.RowStatus == 0);
        }
    }
}
