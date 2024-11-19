using NET.Starter.API.Shared.Constants;
using NET.Starter.API.Shared.Enums;

namespace NET.Starter.API.Shared.Objects.Dtos
{
    internal class ResponseBase
    {
        public ResponseBase(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest)
        {
            switch (responseCode)
            {
                case ResponseCode.Ok:
                    OK(message);
                    break;

                case ResponseCode.BadRequest:
                    BadRequest(message);
                    break;

                case ResponseCode.UnAuthorized:
                    UnAuthorized(message);
                    break;

                case ResponseCode.Forbidden:
                    Forbidden(message);
                    break;

                case ResponseCode.NotFound:
                    NotFound(message);
                    break;

                case ResponseCode.TimeOut:
                    TimeOut(message);
                    break;

                case ResponseCode.Error:
                    Error(message);
                    break;

                default:
                    break;
            }
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public int Code { get; internal set; }
        public bool Succeeded { get; internal set; }
        public string? Message { get; set; }

        #region Public Methods
        public void OK(string? message)
        {
            Code = (int)ResponseCode.Ok;
            Succeeded = true;
            Message = message ?? MessageConstants.StatusOk;
        }

        public void BadRequest(string? message)
        {
            Code = (int)ResponseCode.BadRequest;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusBadRequest;
        }

        public void UnAuthorized(string? message)
        {
            Code = (int)ResponseCode.UnAuthorized;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusUnauthorized;
        }

        public void Forbidden(string? message)
        {
            Code = (int)ResponseCode.Forbidden;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusForbidden;
        }

        public void NotFound(string? message)
        {
            Code = (int)ResponseCode.NotFound;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusNotFound;
        }

        public void TimeOut(string? message)
        {
            Code = (int)ResponseCode.TimeOut;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusTimeOut;
        }

        public void Error(string? message)
        {
            Code = (int)ResponseCode.Error;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusError;
        }
        #endregion
    }
}
