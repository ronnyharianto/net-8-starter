using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="Permission"/> entity.
    /// This class defines property configurations and seeds initial data for the Permission entity.
    /// </summary>
    public class PermissionEntityBuilder : EntityBaseBuilder<Permission>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="Permission"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Call the base configuration for the Permission entity.
            base.Configure(builder);

            builder
                .Property(e => e.PermissionCode)
                .HasMaxLength(50);

            builder
                .HasIndex(e => e.PermissionCode)
                .HasFilter("[RowStatus] = 0")
                .IsUnique();

            SeedingData(builder);
        }

        /// <summary>
        /// Seeds initial data into the Permission table.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{Permission}"/> used to configure the entity type.</param>
        private static void SeedingData(EntityTypeBuilder<Permission> builder)
        {
            // Adds predefined data for the identity permission.
            builder.HasData(
                new Permission { Id = new Guid("69821b03-b132-4d35-88f0-5502908d50fa"), PermissionCode = PermissionConstants.Identity.Admin, Created = new DateTime(2025, 2, 12, 13, 30, 00) }
            );

            // Adds predefined data for the security permission.
            builder.HasData(
                new Permission { Id = new Guid("a0d9d4d0-1b6f-4e4c-8e5e-a7c6a3b2abf1"), PermissionCode = PermissionConstants.Security.Permission.View, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                
                new Permission { Id = new Guid("1a010b41-b5bf-4e15-8042-94bee253c835"), PermissionCode = PermissionConstants.Security.Role.Menu, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("e298741f-3027-4299-bf56-66bd712219e0"), PermissionCode = PermissionConstants.Security.Role.View, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("a8b4b425-8827-4fde-b2c5-1fc3e059f061"), PermissionCode = PermissionConstants.Security.Role.Create, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("ef4baa13-1ed0-4284-8a77-2f6e315aecf8"), PermissionCode = PermissionConstants.Security.Role.Update, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("b3a934db-1d2b-40fd-b6df-00b9594637ce"), PermissionCode = PermissionConstants.Security.Role.Delete, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                
                new Permission { Id = new Guid("11183f54-13f3-4c4c-b484-bd170342ca70"), PermissionCode = PermissionConstants.Security.User.Menu, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("7d8b693b-a21f-4f7e-a0f9-b1f80a86e0bd"), PermissionCode = PermissionConstants.Security.User.View, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("f8568aa0-aae2-4ed0-9783-b75ff180ebdf"), PermissionCode = PermissionConstants.Security.User.Create, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("1bfb9a81-0f01-4981-b4d5-fce12f8b4d4d"), PermissionCode = PermissionConstants.Security.User.Update, Created = new DateTime(2025, 2, 12, 13, 30, 00) },
                new Permission { Id = new Guid("3c390638-d417-421a-a99f-611591c07a4d"), PermissionCode = PermissionConstants.Security.User.Delete, Created = new DateTime(2025, 2, 12, 13, 30, 00) },

                new Permission { Id = new Guid("9f6aedbf-7102-4551-977b-136c0d2f9795"), PermissionCode = PermissionConstants.Organization.Company.Menu, Created = new DateTime(2025, 5, 16, 12, 32, 00) },
                new Permission { Id = new Guid("70f1b765-b00f-4372-8e7d-3858a019724c"), PermissionCode = PermissionConstants.Organization.Company.View, Created = new DateTime(2025, 5, 16, 12, 32, 00) },
                new Permission { Id = new Guid("c890b98e-2c1a-4a8b-a43f-0079e5064c80"), PermissionCode = PermissionConstants.Organization.Company.Create, Created = new DateTime(2025, 5, 16, 12, 32, 00) },
                new Permission { Id = new Guid("c668b06d-da4f-4c8d-aa27-3c2737c30904"), PermissionCode = PermissionConstants.Organization.Company.Update, Created = new DateTime(2025, 5, 16, 12, 32, 00) },
                new Permission { Id = new Guid("ee38dda5-52ce-45a5-ba94-8dfb75704778"), PermissionCode = PermissionConstants.Organization.Company.Delete, Created = new DateTime(2025, 5, 16, 12, 32, 00) },

                new Permission { Id = new Guid("040306c7-db67-47ca-b942-fe786a1eaafd"), PermissionCode = PermissionConstants.Organization.Branch.Menu, Created = new DateTime(2025, 5, 16, 12, 49, 00) },
                new Permission { Id = new Guid("93caa8ca-bdbb-4cfd-92e5-a42c8b5fd510"), PermissionCode = PermissionConstants.Organization.Branch.View, Created = new DateTime(2025, 5, 16, 12, 49, 00) },
                new Permission { Id = new Guid("c8fc725c-1d1e-424a-a533-ae57eb7ee518"), PermissionCode = PermissionConstants.Organization.Branch.Create, Created = new DateTime(2025, 5, 16, 12, 49, 00) },
                new Permission { Id = new Guid("697046ea-6118-48ac-a9b7-c0652aa76262"), PermissionCode = PermissionConstants.Organization.Branch.Update, Created = new DateTime(2025, 5, 16, 12, 49, 00) },
                new Permission { Id = new Guid("53632736-e1d9-428d-8bc5-09b75cf8f4e8"), PermissionCode = PermissionConstants.Organization.Branch.Delete, Created = new DateTime(2025, 5, 16, 12, 49, 00) }
            );
        }
    }
}
