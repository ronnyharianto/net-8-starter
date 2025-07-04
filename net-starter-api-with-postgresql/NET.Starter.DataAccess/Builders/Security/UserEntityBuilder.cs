using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Helpers;

namespace NET.Starter.DataAccess.Builders.Security
{
    internal class UserEntityBuilder : EntityBaseBuilder<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Username)
                .HasMaxLength(50);

            builder
                .Property(e => e.EmailAddress)
                .HasMaxLength(50);

            builder
                .Property(e => e.Password)
                .HasConversion(v => CryptographyHelper.HashPassword(v), v => v);

            builder
                .Property(e => e.FullName)
                .HasMaxLength(50);

            builder
                .HasIndex(e => e.Username)
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();

            builder
                .HasIndex(e => e.EmailAddress)
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();
        }
    }
}
