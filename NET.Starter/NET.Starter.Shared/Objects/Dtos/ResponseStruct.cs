using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Objects.Dtos
{
    public class ResponseStruct<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest) : ResponseBase(message, responseCode)
        where T : struct
    {
        public T? Obj { get; set; }
    }
}
