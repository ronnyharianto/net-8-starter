using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    public class PermissionEntityBuilder : EntityBaseBuilder<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.PermissionCode)
                .HasMaxLength(50);

            builder
                .Property(e => e.PermissionDescription)
                .HasMaxLength(150);

            SeedingData(builder);
        }

        private static void SeedingData(EntityTypeBuilder<Permission> builder)
        {
            builder
                .HasData(
                    new Permission { Id = new Guid("6659f17a-c52e-4ec3-847b-46866a3b2abf"), PermissionCode = PermissionConstants.MyPermission, PermissionDescription = "Ijin untuk mendapatkan data permission atas user login", Created = new DateTime(2024, 11, 20, 11, 00, 00) }
                );
        }
    }
}
