using NET.Starter.API.Shared.Enums;

namespace NET.Starter.API.Shared.Objects.Dtos
{
    public class ResponseStruct<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest) : ResponseBase(message, responseCode)
        where T : struct
    {
        public T? Obj { get; set; }
    }
}
