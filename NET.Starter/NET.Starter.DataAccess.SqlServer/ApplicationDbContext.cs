using Microsoft.EntityFrameworkCore;
using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Builders.Security;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Objects;

namespace NET.Starter.DataAccess.SqlServer
{
    public class ApplicationDbContext(DbContextOptions options, CurrentUserAccessor currentUserAccessor) : DbContextBase(options, currentUserAccessor)
    {
        #region Security
        public virtual DbSet<Permission> Permissions { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Security
            new PermissionEntityBuilder().Configure(modelBuilder.Entity<Permission>());
            #endregion
        }
    }
}
