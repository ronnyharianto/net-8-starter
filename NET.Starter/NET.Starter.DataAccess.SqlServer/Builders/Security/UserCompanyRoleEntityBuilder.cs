using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.DataAccess.SqlServer.Builders.Security
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="UserCompanyRole"/> entity.
    /// This class defines property configurations and seeds initial data for the UserCompanyRole entity.
    /// </summary>
    public class UserCompanyRoleEntityBuilder : EntityBaseBuilder<UserCompanyRole>
    {
        /// <summary>
        /// Configures the properties and relationships of the <see cref="UserCompanyRole"/> entity.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public override void Configure(EntityTypeBuilder<UserCompanyRole> builder)
        {
            // Call the base configuration for the Permission entity.
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
                .HasFilter("[RowStatus] = 0")
                .IsUnique();
        }
    }
}
