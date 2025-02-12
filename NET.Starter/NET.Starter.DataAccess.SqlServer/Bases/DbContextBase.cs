using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects;

namespace NET.Starter.DataAccess.SqlServer.Bases
{
    /// <summary>
    /// Base class for Entity Framework DbContext, providing additional functionality such as automatic 
    /// tracking of created and modified timestamps and assigning actor information.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DbContextBase"/> class.
    /// </remarks>
    /// <param name="options">The options to be used by this DbContext.</param>
    /// <param name="currentUserAccessor">The accessor to retrieve the current user's information.</param>
    public class DbContextBase(DbContextOptions options, CurrentUserAccessor currentUserAccessor) : DbContext(options)
    {
        // Reference to the accessor that provides information about the current user.
        private readonly CurrentUserAccessor _currentUserAccessor = currentUserAccessor;

        /// <summary>
        /// Saves all changes made in this context to the database, while automatically updating 
        /// created and modified timestamps, as well as actor information.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            UpdateActorAndTimestamps();

            return base.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database, while automatically updating 
        /// created and modified timestamps, as well as actor information.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateActorAndTimestamps();

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates created and modified timestamps, and actor information for entities being added or modified.
        /// </summary>
        private void UpdateActorAndTimestamps()
        {
            // Process entities in the Added state.
            var createdEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var entry in createdEntries)
            {
                if (entry.Entity is EntityBase entity)
                {
                    entity.Created = DateTime.UtcNow;
                    entity.CreatedBy = _currentUserAccessor.UserId.ToString();
                }
            }

            // Process entities in the Modified state.
            var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is EntityBase entity)
                {
                    entity.Modified = DateTime.UtcNow;
                    entity.ModifiedBy = _currentUserAccessor.UserId.ToString();
                }
            }
        }

        /// <summary>
        /// Configures the schema and table name for all DbSet properties based on their 
        /// <see cref="DatabaseSchemaAttribute"/> attributes or defaults to the "dbo" schema.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Retrieve all DbSet properties from the current DbContext.
            var dbSetProperties = GetType().GetProperties()
                                           .Where(p =>
                                               p.PropertyType.IsGenericType &&
                                               p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProperty in dbSetProperties)
            {
                // Determine the entity type and its schema.
                var entityType = dbSetProperty.PropertyType.GenericTypeArguments[0];
                var schemaAttribute = entityType.GetCustomAttributes(typeof(DatabaseSchemaAttribute), false).FirstOrDefault() as DatabaseSchemaAttribute;
                var schema = schemaAttribute?.Schema ?? "dbo";

                // Use the property name as the table name.
                var tableName = dbSetProperty.Name;

                // Map the entity to the table with the specified schema.
                modelBuilder.Entity(entityType).ToTable(tableName, schema);
            }
        }
    }
}