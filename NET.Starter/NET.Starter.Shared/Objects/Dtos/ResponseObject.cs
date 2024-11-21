using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Objects.Dtos
{
    public class ResponseObject<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest) : ResponseBase(message, responseCode)
        where T : class
    {
        public T? Obj { get; set; }
    }
}
