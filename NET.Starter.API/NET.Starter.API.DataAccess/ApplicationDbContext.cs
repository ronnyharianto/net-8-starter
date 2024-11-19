using Microsoft.EntityFrameworkCore;
using NET.Starter.API.DataAccess.Bases;
using NET.Starter.API.Shared.Objects;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET.Starter.API.Core")]

namespace NET.Starter.API.DataAccess
{
    internal class ApplicationDbContext(DbContextOptions options, CurrentUserAccessor currentUserAccessor) : DbContextBase(options, currentUserAccessor)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
