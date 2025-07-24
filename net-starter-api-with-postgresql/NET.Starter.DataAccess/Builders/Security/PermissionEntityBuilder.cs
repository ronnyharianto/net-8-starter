using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Constants;

namespace NET.Starter.DataAccess.Builders.Security
{
    internal class PermissionEntityBuilder : EntityBaseBuilder<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Code)
                .HasMaxLength(100);

            builder
                .HasIndex(e => e.Code)
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();

            SeedingData_20250724_0300(builder);
        }

        private static void SeedingData_20250724_0300(EntityTypeBuilder<Permission> builder)
        {
            var permissions = new Permission[]
            {
                // General
                new() { Id = new Guid("d6d432c4-752d-4f64-95c0-ec10b174a4cf"), Code = PermissionConstants.RetrieveFileFromStorage },
                
                // Identity
                new() { Id = new Guid("69821b03-b132-4d35-88f0-5502908d50fa"), Code = PermissionConstants.Identity.IamAdministrator },

                // Role
                new() { Id = new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"), Code = PermissionConstants.Security.Role.Access },
                new() { Id = new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"), Code = PermissionConstants.Security.Role.Modify },
                new() { Id = new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"), Code = PermissionConstants.Security.Role.Delete },

                // User
                new() { Id = new Guid("11183f54-13f3-4c4c-b484-bd170342ca70"), Code = PermissionConstants.Security.User.Access },
                new() { Id = new Guid("1bfb9a81-0f01-4981-b4d5-fce12f8b4d4d"), Code = PermissionConstants.Security.User.Modify },
                new() { Id = new Guid("3c390638-d417-421a-a99f-611591c07a4d"), Code = PermissionConstants.Security.User.Delete },

                // Company
                new() { Id = new Guid("9f6aedbf-7102-4551-977b-136c0d2f9795"), Code = PermissionConstants.Organization.Company.Access },
                new() { Id = new Guid("c668b06d-da4f-4c8d-aa27-3c2737c30904"), Code = PermissionConstants.Organization.Company.Modify },
                new() { Id = new Guid("ee38dda5-52ce-45a5-ba94-8dfb75704778"), Code = PermissionConstants.Organization.Company.Delete }
            };

            foreach (var permission in permissions)
            {
                permission.Created = new(2025, 7, 24, 3, 0, 0, DateTimeKind.Utc);
                permission.CreatedBy = GeneralConstants.EFCoreMigration.CreatedBy;
            }

            builder.HasData(permissions);
        }
    }
}
