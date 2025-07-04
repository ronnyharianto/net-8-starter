using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Enums;

namespace NET.Starter.DataAccess.Models.System
{
    /// <summary>
    /// Represents a document numbering configuration used to generate sequential document numbers.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description><see cref="Prefix"/> is unique and required.</description></item>
    ///   <item><description>Tracks numbering by <see cref="DocumentType"/> and reset period (<see cref="ResetPeriode"/>).</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("system")]
    internal class DocumentNumbering : EntityBase
    {
        /// <summary>
        /// Stored as a string in the database.
        /// Maximum length: 25 characters.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Period after which numbering resets (e.g., monthly, yearly).
        /// Stored as a string in the database.
        /// Maximum length: 10 characters.
        /// </summary>
        public Period ResetPeriode { get; set; }

        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public required string Prefix { get; set; }

        public int LastNumber { get; set; }

        /// <summary>
        /// Number of digits to pad the numeric on last part of the document number.
        /// </summary>
        public int Padding { get; set; }
    }
}
