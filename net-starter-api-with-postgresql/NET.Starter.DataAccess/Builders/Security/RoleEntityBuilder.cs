using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Constants;

namespace NET.Starter.DataAccess.Builders.Security
{
    internal class RoleEntityBuilder : EntityBaseBuilder<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Code)
                .HasMaxLength(50);

            builder
                .HasIndex(e => e.Code)
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();

            SeedingData_20250724_0300(builder);
        }

        private static void SeedingData_20250724_0300(EntityTypeBuilder<Role> builder)
        {
            var role = new Role { Id = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), Code = "Administrator", Created = new(2025, 7, 24, 3, 0, 0, DateTimeKind.Utc), CreatedBy = GeneralConstant.EFCoreMigration.CreatedBy };

            builder.HasData(role);
        }
    }
}
