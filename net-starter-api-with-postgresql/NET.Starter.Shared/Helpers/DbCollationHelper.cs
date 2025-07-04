using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Provides helper methods to retrieve collation names
    /// based on the target database engine and comparison type.
    /// </summary>
    public static class DbCollationHelper
    {
        /// <summary>
        /// Gets the default collation for case-insensitive string comparisons,
        /// based on the specified database engine.
        /// </summary>
        /// <param name="engine">The database engine in use.</param>
        /// <returns>The name of the case-insensitive collation.</returns>
        public static string GetCaseInsensitiveCollation(DatabaseEngine engine) =>
            engine switch
            {
                DatabaseEngine.SQLServer => CollationConstants.SQL_Latin1_General_CP1_CI_AS,
                DatabaseEngine.PostgreSQL => CollationConstants.PG_English_UnitedStates_CI,
                _ => throw new NotSupportedException($"Unsupported database engine: {engine}")
            };

        /// <summary>
        /// Gets the default collation for case-sensitive string comparisons,
        /// based on the specified database engine.
        /// </summary>
        /// <param name="engine">The database engine in use.</param>
        /// <returns>The name of the case-sensitive collation.</returns>
        public static string GetCaseSensitiveCollation(DatabaseEngine engine) =>
            engine switch
            {
                DatabaseEngine.SQLServer => CollationConstants.SQL_Latin1_General_CP1_CS_AS,
                DatabaseEngine.PostgreSQL => CollationConstants.PG_POSIX_C,
                _ => throw new NotSupportedException($"Unsupported database engine: {engine}")
            };
    }
}
