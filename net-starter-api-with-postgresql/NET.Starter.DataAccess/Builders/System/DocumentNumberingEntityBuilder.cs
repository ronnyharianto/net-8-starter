using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.System;

namespace NET.Starter.DataAccess.Builders.System
{
    internal class DocumentNumberingEntityBuilder : EntityBaseBuilder<DocumentNumbering>
    {
        public override void Configure(EntityTypeBuilder<DocumentNumbering> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.DocumentType)
                .HasConversion<string>()
                .HasMaxLength(25);

            builder
                .Property(e => e.ResetPeriode)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder
                .Property(e => e.Prefix)
                .HasMaxLength(50);

            builder
                .HasIndex(e => new { e.Prefix })
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();
        }
    }
}
