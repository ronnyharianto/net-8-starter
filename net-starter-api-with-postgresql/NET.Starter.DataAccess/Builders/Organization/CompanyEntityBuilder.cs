using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Organization;

namespace NET.Starter.DataAccess.Builders.Organization
{
    internal class CompanyEntityBuilder : EntityBaseBuilder<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Code)
                .HasMaxLength(15);

            builder
                .Property(e => e.Name)
                .HasMaxLength(50);

            builder
                .Property(e => e.Address)
                .HasMaxLength(150);

            builder
                .Property(e => e.PhoneNumber)
                .HasMaxLength(15);

            builder
                .Property(e => e.Email)
                .HasMaxLength(50);

            builder
                .Property(e => e.Website)
                .HasMaxLength(50);

            builder
                .HasIndex(e => e.Code)
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();
        }
    }
}
