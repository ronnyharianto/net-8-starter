using NET.Starter.API.Shared.Enums;

namespace NET.Starter.API.Shared.Objects.Dtos
{
    internal class ResponseStruct<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest) : ResponseBase(message, responseCode)
        where T : struct
    {
        public T? Obj { get; set; }
    }
}
