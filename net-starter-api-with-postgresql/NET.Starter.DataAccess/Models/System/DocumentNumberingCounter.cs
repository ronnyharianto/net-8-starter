using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.System
{
    /// <summary>
    /// Represents the running document number state for a specific
    /// <see cref="DocumentNumbering"/> configuration and period.
    /// <para>
    /// This entity stores the last generated number to ensure sequential
    /// and unique document numbers within a defined period.
    /// </para>
    /// <para><b>Design notes:</b></para>
    /// <list type="bullet">
    ///   <item><description>This table stores <b>state</b>, not configuration.</description></item>
    ///   <item><description>Each record is uniquely identified by the combination of<see cref="DocumentNumberingId"/> and <see cref="PeriodKey"/>.</description></item>
    ///   <item><description>Designed to support safe concurrent updates during document number generation.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("system")]
    internal class DocumentNumberingCounter : EntityBase
    {
        /// <summary>
        /// Foreign key reference to the associated
        /// <see cref="DocumentNumbering"/> configuration.
        /// </summary>
        public Guid DocumentNumberingId { get; set; }

        /// <summary>
        /// Represents the period for which the running number applies.
        /// <para>
        /// Examples:
        /// </para>
        /// <list type="bullet">
        ///   <item><description>Daily  : 2026-01-20</description></item>
        ///   <item><description>Monthly: 2026-01-01</description></item>
        ///   <item><description>Yearly : 2026-01-01</description></item>
        /// </list>
        /// The exact interpretation depends on the reset period defined
        /// in <see cref="DocumentNumbering"/>.
        /// </summary>
        public DateOnly PeriodKey { get; set; }

        /// <summary>
        /// Stores the last generated numeric value for the given period.
        /// </summary>
        /// <remarks>
        /// This value is incremented atomically when a new document number
        /// is generated to ensure uniqueness.
        /// </remarks>
        public int LastNumber { get; set; }

        /// <summary>
        /// Navigation property to the related
        /// <see cref="DocumentNumbering"/> configuration.
        /// </summary>
        public virtual DocumentNumbering DocumentNumbering { get; set; } = null!;
    }
}
