using System.ComponentModel.DataAnnotations;

namespace NET.Starter.DataAccess.SqlServer.Bases
{
    /// <summary>
    /// Base class for all entities, providing common properties such as primary key, 
    /// timestamps for tracking creation and modifications, and a soft delete indicator.
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Primary key for the entity, uniquely identifying each record.
        /// Automatically generated as a new GUID.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The date and time when the record was created.
        /// Automatically assigned during entity creation.
        /// </summary>
        public DateTime Created { get; internal set; }

        /// <summary>
        /// The user identifier of the actor who created the record.
        /// </summary>
        public string CreatedBy { get; internal set; } = string.Empty;

        /// <summary>
        /// The date and time when the record was last modified.
        /// Null if the record has not been modified since creation.
        /// </summary>
        public DateTime? Modified { get; internal set; }

        /// <summary>
        /// The user identifier of the actor who last modified the record.
        /// Null if the record has not been modified since creation.
        /// </summary>
        public string? ModifiedBy { get; internal set; }

        /// <summary>
        /// Status of the record to indicate active or soft-deleted data.
        /// <para>0: Active data</para>
        /// <para>1: Soft-deleted data</para>
        /// </summary>
        public int RowStatus { get; set; } = 0;
    }
}
