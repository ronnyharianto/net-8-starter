using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;

namespace NET.Starter.DataAccess.Builders.Security
{
    internal class UserPushTokenEntityBuilder : EntityBaseBuilder<UserPushToken>
    {
        public override void Configure(EntityTypeBuilder<UserPushToken> builder)
        {
            base.Configure(builder);

            builder
                .Property(e => e.Provider)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.UserPushTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(e => new { e.Provider, e.PushToken })
                .HasFilter("\"RowStatus\" = 0")
                .IsUnique();
        }
    }
}
