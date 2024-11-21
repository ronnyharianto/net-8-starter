using Microsoft.EntityFrameworkCore;
using NET.Starter.API.DataAccess.SqlServer.Bases;
using NET.Starter.API.DataAccess.SqlServer.Builders.Security;
using NET.Starter.API.DataAccess.SqlServer.Models.Security;
using NET.Starter.API.Shared.Objects;

namespace NET.Starter.API.DataAccess.SqlServer
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
