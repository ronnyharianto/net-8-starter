using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects;

namespace NET.Starter.DataAccess.SqlServer.Bases
{
    public class DbContextBase(DbContextOptions options, CurrentUserAccessor currentUserAccessor) : DbContext(options)
    {
        private readonly CurrentUserAccessor _currentUserAccessor = currentUserAccessor;

        public override int SaveChanges()
        {
            UpdateActorAndTimestamps();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateActorAndTimestamps();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateActorAndTimestamps()
        {
            var createdEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var entry in createdEntries)
            {
                if (entry.Entity is EntityBase entity)
                {
                    entity.Created = DateTime.Now;
                    entity.CreatedBy = _currentUserAccessor.Id.ToString();
                }
            }

            var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is EntityBase entity)
                {
                    entity.Modified = DateTime.Now;
                    entity.ModifiedBy = _currentUserAccessor.Id.ToString();
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dbSetProperties = GetType().GetProperties()
                                           .Where(p => 
                                            p.PropertyType.IsGenericType &&
                                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProperty in dbSetProperties)
            {
                var entityType = dbSetProperty.PropertyType.GenericTypeArguments[0];
                var schemaAttribute = entityType.GetCustomAttributes(typeof(DatabaseSchemaAttribute), false).FirstOrDefault() as DatabaseSchemaAttribute;
                var schema = schemaAttribute?.Schema ?? "dbo";
                var tableName = dbSetProperty.Name;

                modelBuilder.Entity(entityType).ToTable(tableName, schema);
            }
        }
    }
}
