using System.ComponentModel.DataAnnotations;

namespace NET.Starter.DataAccess.Bases
{
    /// <summary>
    /// Base class for all entities, providing common properties like Id, audit timestamps, and soft delete status.
    /// </summary>
    internal class EntityBase
    {
        /// <summary>
        /// Unique identifier for the entity.
        /// Automatically initialized with a new GUID.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// UTC timestamp when the entity was created.
        /// Set internally and not modifiable from outside.
        /// </summary>
        public DateTime Created { get; internal set; }

        /// <summary>
        /// Identifier of the user who created the entity.
        /// Set internally and not modifiable from outside.
        /// </summary>
        public string CreatedBy { get; internal set; } = string.Empty;

        /// <summary>
        /// UTC timestamp when the entity was last modified.
        /// Null if the entity has never been modified.
        /// Set internally and not modifiable from outside.
        /// </summary>
        public DateTime? Modified { get; internal set; }

        /// <summary>
        /// Identifier of the user who last modified the entity.
        /// Null if the entity has never been modified.
        /// Set internally and not modifiable from outside.
        /// </summary>
        public string? ModifiedBy { get; internal set; }

        /// <summary>
        /// Status flag indicating whether the record is active or soft-deleted.
        /// <list type="bullet">
        /// <item><description>0 - Active data</description></item>
        /// <item><description>1 - Soft-deleted data</description></item>
        /// </list>
        /// </summary>
        public int RowStatus { get; set; } = 0;
    }
}
