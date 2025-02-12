using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a response that contains a payload of a value type.
    /// </summary>
    /// <typeparam name="T">The value type of the response payload.</typeparam>
    public class StructDto<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest)
        : BaseDto(message, responseCode)
        where T : struct
    {
        /// <summary>
        /// The payload containing the value type data.
        /// </summary>
        public T Obj { get; set; }
    }
}
