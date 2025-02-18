using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to mark a method as a mutation operation.
    /// Typically applied in scenarios where the method represents a change or modification 
    /// to the underlying data in transactional operations.
    /// </summary>
    /// <remarks>
    /// The <see cref="MutationAttribute"/> is used to identify methods that represent mutation 
    /// operations in the application. When applied, the attribute specifies a list of allowed 
    /// response codes (<see cref="AllowedResponseCodes"/>). 
    ///
    /// Each response code listed in <see cref="AllowedResponseCodes"/> signifies that, upon 
    /// encountering the corresponding response during the execution of the method, the middleware 
    /// will commit the database transaction rather than rolling it back. If no response codes are 
    /// explicitly defined, the attribute defaults to allowing <see cref="ResponseCode.Ok"/> only.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MutationAttribute"/> class.
    /// </remarks>
    /// <param name="allowedResponseCodes">
    /// An optional array of allowed response codes. If <c>null</c> or not provided, 
    /// the default behavior allows only <see cref="ResponseCode.Ok"/> to trigger a commit.
    /// </param>
    [AttributeUsage(AttributeTargets.Method)]
    public class MutationAttribute(ResponseCode[]? allowedResponseCodes = null) : Attribute
    {
        /// <summary>
        /// Gets the list of allowed response codes for the mutation operation.
        /// </summary>
        /// <remarks>
        /// For each response code included in this array, the middleware will commit 
        /// the database transaction when the corresponding response is encountered. 
        /// If the response code is not included, the transaction will be rolled back by default.
        /// </remarks>
        public int[] AllowedResponseCodes { get; } = (allowedResponseCodes?.Cast<int>() ?? []).Append((int)ResponseCode.Ok).Distinct().ToArray();
    }
}
