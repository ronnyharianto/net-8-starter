using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Constants;

namespace NET.Starter.DataAccess.Builders.Security
{
    internal class RolePermissionEntityBuilder : EntityBaseBuilder<RolePermission>
    {
        public override void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(e => e.Role)
                .WithMany(e => e.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Permission)
                .WithMany(e => e.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => new { e.RoleId, e.PermissionId })
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();

            SeedingData_Administrator_20250724_0300(builder);
        }

        private static void SeedingData_Administrator_20250724_0300(EntityTypeBuilder<RolePermission> builder)
        {
            var rolePermissions = new RolePermission[]
            {
                new() { Id = new Guid("02abe492-24b8-4a90-af18-3a282f9fcc85"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("69821b03-b132-4d35-88f0-5502908d50fa") },
                new() { Id = new Guid("3895d9f6-d69d-44ac-a9a3-2d8634c44d9e"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("d6d432c4-752d-4f64-95c0-ec10b174a4cf") },

                #region Permission for Role

                new() { Id = new Guid("2f0e1598-1ba3-495d-9ebe-673b15512c60"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("1a010b41-b5bf-4e15-8042-94bee253c835") },
                new() { Id = new Guid("223c7549-ddcc-4d05-95dc-9336c76a3e57"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8") },
                new() { Id = new Guid("c28724cb-c0c1-462c-8a71-3161dc638920"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce") },

                #endregion

                #region Permission for User
                
                new() { Id = new Guid("b6f275c7-1732-4289-90bc-30b75dfdb165"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("11183f54-13f3-4c4c-b484-bd170342ca70") },
                new() { Id = new Guid("eeb6d346-4e4b-4273-b030-9a15218bf037"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("1bfb9a81-0f01-4981-b4d5-fce12f8b4d4d") },
                new() { Id = new Guid("c686e3ae-9a80-4474-872a-33340d19961f"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("3c390638-d417-421a-a99f-611591c07a4d") },
                
                #endregion

                #region Permission for Company

                new() { Id = new Guid("4db012d0-ee9e-4318-8c61-0f705b6768f4"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("9f6aedbf-7102-4551-977b-136c0d2f9795") },
                new() { Id = new Guid("e72ca2b3-f609-4101-84e2-a1cb2fdbe087"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("c668b06d-da4f-4c8d-aa27-3c2737c30904") },
                new() { Id = new Guid("67148cfa-2023-4469-bfb9-df2d75935072"), RoleId = new Guid("3bafc714-4aa5-4fc3-8542-f4eeb798f918"), PermissionId = new Guid("ee38dda5-52ce-45a5-ba94-8dfb75704778") }

                #endregion
            };

            foreach (var rolePermission in rolePermissions)
            {
                rolePermission.Created = new(2025, 7, 24, 3, 0, 0, DateTimeKind.Utc);
                rolePermission.CreatedBy = GeneralConstants.EFCoreMigration.CreatedBy;
            }

            builder.HasData(rolePermissions);
        }
    }
}
