using System.Net;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a response that contains a payload of a value type.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the response. Must be a value type.</typeparam>
    /// <param name="message">An optional message describing the response.</param>
    /// <param name="httpStatusCode">The HTTP status code representing the response status.</param>
    public class StructDto<T>(string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : BaseDto(message, httpStatusCode)
        where T : struct
    {
        /// <summary>
        /// The response data value.
        /// </summary>
        public T Obj { get; set; }
    }
}
