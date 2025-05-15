using System.Net;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a response that contains a payload of a value type.
    /// </summary>
    public class StructDto<T>(string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : BaseDto(message, httpStatusCode)
        where T : struct
    {
        /// <summary>
        /// The payload containing the value type data.
        /// </summary>
        public T Obj { get; set; }
    }
}
