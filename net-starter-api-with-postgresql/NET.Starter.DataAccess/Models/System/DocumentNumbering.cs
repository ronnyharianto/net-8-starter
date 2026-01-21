using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Enums;

namespace NET.Starter.DataAccess.Models.System
{
    /// <summary>
    /// Represents a document numbering configuration used to generate sequential document numbers.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description><see cref="DocumentType"/> is unique and required.</description></item>
    ///   <item><description>Numbering is tracked by <see cref="DocumentType"/> and reset period (<see cref="ResetPeriode"/>).</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("system")]
    internal class DocumentNumbering : EntityBase
    {
        /// <summary>
        /// Defines the type of document this numbering configuration applies to.
        /// </summary>
        /// <remarks>
        /// Stored as a string in the database.
        /// Maximum length: 25 characters.
        /// </remarks>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Defines the period after which the numbering resets
        /// (e.g. monthly, yearly).
        /// </summary>
        /// <remarks>
        /// Stored as a string in the database.
        /// Maximum length: 10 characters.
        /// </remarks>
        public Period ResetPeriode { get; set; }

        /// <summary>
        /// Prefix used at the beginning of the generated document number.
        /// </summary>
        /// <remarks>
        /// Maximum length: 50 characters.
        /// </remarks>
        public required string Prefix { get; set; }

        /// <summary>
        /// Number of digits used to pad the numeric portion of the document number.
        /// </summary>
        /// <example>
        /// Padding = 5 → 00001, 00002, etc.
        /// </example>
        public int Padding { get; set; }

        public ICollection<DocumentNumberingCounter> DocumentNumberingCounters { get; set; } = [];
    }
}