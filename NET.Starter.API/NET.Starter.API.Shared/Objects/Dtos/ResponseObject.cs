using NET.Starter.API.Shared.Enums;

namespace NET.Starter.API.Shared.Objects.Dtos
{
    internal class ResponseObject<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest) : ResponseBase(message, responseCode)
        where T : class
    {
        public T? Obj { get; set; }
    }
}
