using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a response object that contains a data payload.
    /// </summary>
    /// <typeparam name="T">The type of the data payload.</typeparam>
    public class ObjectDto<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest)
        : BaseDto(message, responseCode)
        where T : class?
    {
        /// <summary>
        /// Gets or sets the data payload of the response.
        /// </summary>
        public T? Obj { get; set; }
    }
}