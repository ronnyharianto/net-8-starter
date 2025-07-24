using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects;
using Serilog;

namespace NET.Starter.DataAccess.Bases
{
    /// <summary>
    /// Base <see cref="DbContext"/> that automatically tracks audit fields such as Created, CreatedBy, Modified, and ModifiedBy.
    /// It also maps entities to database schemas dynamically based on the <see cref="DatabaseSchemaAttribute"/>.
    /// </summary>
    internal class DbContextBase(DbContextOptions _options, CurrentUserAccessor _currentUserAccessor) : DbContext(_options)
    {
        /// <summary>
        /// Saves all changes made in this context to the database.
        /// Automatically sets audit fields for added and modified entities before saving.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            UpdateActorAndTimestamps();

            Log.Information("Saving changes to database with audit fields updated.");

            return base.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// Automatically sets audit fields for added and modified entities before saving.
        /// </summary>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateActorAndTimestamps();

            Log.Information("Saving changes asynchronously to database with audit fields updated.");

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates audit properties (Created, CreatedBy, Modified, ModifiedBy) on entities tracked by this context.
        /// Sets Created and CreatedBy for new entities, and Modified and ModifiedBy for updated entities.
        /// </summary>
        private void UpdateActorAndTimestamps()
        {
            var createdEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var entry in createdEntries)
            {
                if (entry.Entity is EntityBase entity)
                {
                    entity.Created = DateTime.UtcNow;
                    entity.CreatedBy = _currentUserAccessor.UserId.ToString();
                }
            }

            var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is EntityBase entity)
                {
                    entity.Modified = DateTime.UtcNow;
                    entity.ModifiedBy = _currentUserAccessor.UserId.ToString();
                }
            }

            Log.Debug("Updated audit fields: {CreatedCount} created, {ModifiedCount} modified entries.",
                createdEntries.Count(), modifiedEntries.Count());
        }

        /// <summary>
        /// Configures the model and applies database schema mappings based on <see cref="DatabaseSchemaAttribute"/>.
        /// Defaults to the 'public' schema if none is specified.
        /// </summary>
        /// <param name="modelBuilder">Builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dbSetProperties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                                           .Where(p =>
                                               p.PropertyType.IsGenericType &&
                                               p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProperty in dbSetProperties)
            {
                var entityType = dbSetProperty.PropertyType.GenericTypeArguments[0];
                var schemaAttribute = entityType.GetCustomAttributes(typeof(DatabaseSchemaAttribute), false).FirstOrDefault() as DatabaseSchemaAttribute;
                var schema = schemaAttribute?.Schema ?? "public";

                var tableName = dbSetProperty.Name;

                modelBuilder.Entity(entityType).ToTable(tableName, schema);
            }

            Log.Information("Configured database schema mappings for entities.");
        }
    }
}
