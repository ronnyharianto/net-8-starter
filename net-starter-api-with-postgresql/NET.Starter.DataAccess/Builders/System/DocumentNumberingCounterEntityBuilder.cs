using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.System;

namespace NET.Starter.DataAccess.Builders.System
{
    internal class DocumentNumberingCounterEntityBuilder : EntityBaseBuilder<DocumentNumberingCounter>
    {
        public override void Configure(EntityTypeBuilder<DocumentNumberingCounter> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(e => e.DocumentNumbering)
                .WithMany(e => e.DocumentNumberingCounters)
                .HasForeignKey(e => e.DocumentNumberingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => new { e.DocumentNumberingId, e.PeriodKey })
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();
        }
    }
}
