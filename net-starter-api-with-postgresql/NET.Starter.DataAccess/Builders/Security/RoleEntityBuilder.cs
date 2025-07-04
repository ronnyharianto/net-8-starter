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

            SeedingData(builder);
        }

        private static void SeedingData(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { Id = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), Code = "Administrator", Created = GeneralConstants.EFCoreMigration.Created, CreatedBy = GeneralConstants.EFCoreMigration.CreatedBy }
            );
        }
    }
}
