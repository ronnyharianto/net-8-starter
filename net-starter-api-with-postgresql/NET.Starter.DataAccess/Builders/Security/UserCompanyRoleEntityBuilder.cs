using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;

namespace NET.Starter.DataAccess.Builders.Security
{
    internal class UserCompanyRoleEntityBuilder : EntityBaseBuilder<UserCompanyRole>
    {
        public override void Configure(EntityTypeBuilder<UserCompanyRole> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(e => e.UserCompany)
                .WithMany(e => e.UserCompanyRoles)
                .HasForeignKey(e => e.UserCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Role)
                .WithMany(e => e.UserCompanyRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => new { e.UserCompanyId, e.RoleId })
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();
        }
    }
}
