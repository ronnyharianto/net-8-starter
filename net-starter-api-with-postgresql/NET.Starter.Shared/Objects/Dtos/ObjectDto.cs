using System.Net;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a response containing data along with status and message.
    /// </summary>
    /// <param name="message">An optional message describing the response.</param>
    /// <param name="httpStatusCode">The HTTP status code representing the response status.</param>
    /// <typeparam name="T">The type of data included in the response.</typeparam>
    public class ObjectDto<T>(string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : BaseDto(message, httpStatusCode)
        where T : class?
    {
        /// <summary>
        /// The data included in the response.
        /// </summary>
        public T? Obj { get; set; }
    }
}