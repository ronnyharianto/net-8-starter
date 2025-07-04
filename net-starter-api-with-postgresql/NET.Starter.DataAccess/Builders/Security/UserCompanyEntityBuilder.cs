using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;

namespace NET.Starter.DataAccess.Builders.Security
{
    internal class UserCompanyEntityBuilder : EntityBaseBuilder<UserCompany>
    {
        public override void Configure(EntityTypeBuilder<UserCompany> builder)
        {
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
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();

            builder
                .HasIndex(e => new { e.UserId })
                .HasFilter("\"RowStatus\" = 0 AND \"IsDefault\" = true")
                .IsUnique();
        }
    }
}
