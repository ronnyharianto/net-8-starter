using Microsoft.EntityFrameworkCore;
using NET.Starter.API.DataAccess.Bases;
using NET.Starter.API.Shared.Objects;

namespace NET.Starter.API.DataAccess
{
    public class ApplicationDbContext(DbContextOptions options, CurrentUserAccessor currentUserAccessor) : DbContextBase(options, currentUserAccessor)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
