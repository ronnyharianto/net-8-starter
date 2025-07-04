namespace NET.Starter.Shared.Constants
{
    /// <summary>
    /// General constants
    /// </summary>
    public static class GeneralConstants
    {
        public static class EFCoreMigration
        {
            public const string CreatedBy = "EF Core Migration";

            public static readonly DateTime Created = new(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc);
        }
    }
}