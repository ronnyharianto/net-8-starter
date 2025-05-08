using System.Net;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a response object that contains a data payload.
    /// </summary>
    /// <typeparam name="T">The type of the data payload.</typeparam>
    public class ObjectDto<T>(string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : BaseDto(message, httpStatusCode)
        where T : class?
    {
        /// <summary>
        /// Data payload of the response.
        /// </summary>
        public T? Obj { get; set; }
    }
}