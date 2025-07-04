namespace NET.Starter.Shared.Constants
{
    /// <summary>
    /// Contains constant keys for custom JWT claims used throughout the application.
    /// </summary>
    public static class CustomClaimTypeConstants
    {
        /// <summary>
        /// The claim that stores the Id of the company the user is currently accessing.
        /// This value may change if the user switches companies within the application.
        /// </summary>
        public const string CurrentCompany = "current_company";

        /// <summary>
        /// The claim that stores a list of permission codes (type codes) associated with the authenticated user.
        /// This is typically used for authorization checks throughout the system.
        /// </summary>
        public const string TypeCode = "permissions";
    }
}
