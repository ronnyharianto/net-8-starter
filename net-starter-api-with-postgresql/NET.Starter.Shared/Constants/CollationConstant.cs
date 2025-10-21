namespace NET.Starter.Shared.Constants
{
    /// <summary>
    /// Common collation names for SQL Server and PostgreSQL.
    /// </summary>
    internal static class CollationConstant
    {
        /// <summary>
        /// SQL Server collation for case-insensitive comparisons.
        /// </summary>
        internal const string SQL_Latin1_General_CP1_CI_AS = "SQL_Latin1_General_CP1_CI_AS";

        /// <summary>
        /// SQL Server collation for case-sensitive comparisons.
        /// </summary>
        internal const string SQL_Latin1_General_CP1_CS_AS = "SQL_Latin1_General_CP1_CS_AS";

        /// <summary>
        /// PostgreSQL collation using en_US.UTF-8 (generally case-insensitive, locale-based).
        /// </summary>
        internal const string PG_English_UnitedStates_CI = "en_US.UTF-8";

        /// <summary>
        /// PostgreSQL POSIX collation (binary sort, case-sensitive).
        /// </summary>
        internal const string PG_POSIX_C = "C";
    }
}
