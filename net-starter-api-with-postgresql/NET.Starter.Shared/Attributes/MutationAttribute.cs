using System.Net;

namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Marks a method as a mutation operation that can change data.
    /// This attribute informs middleware to commit or rollback a transaction
    /// based on the HTTP status code returned by the method.
    /// </summary>
    /// <param name="acceptedHttpStatusCodes">
    /// Optional list of HTTP status codes that are considered successful for committing the transaction.
    /// If not specified or null, only HTTP 200 (OK) is considered successful.
    /// </param>
    [AttributeUsage(AttributeTargets.Method)]
    public class MutationAttribute(HttpStatusCode[]? acceptedHttpStatusCodes = null) : Attribute
    {
        /// <summary>
        /// Gets the HTTP status codes that will trigger committing the transaction.
        /// Status codes not in this list will cause the transaction to roll back.
        /// HTTP 200 (OK) is always included.
        /// </summary>
        public int[] AcceptedResponseCodes { get; } =
            [.. (acceptedHttpStatusCodes?.Cast<int>() ?? []).Append((int)HttpStatusCode.OK).Distinct()];
    }
}
