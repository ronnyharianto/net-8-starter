using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a base response class for API responses.
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDto"/> class with a specified response code and an optional message.
        /// </summary>
        /// <param name="message">The response message.</param>
        /// <param name="responseCode">The response code indicating the result.</param>
        public BaseDto(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest)
        {
            switch (responseCode)
            {
                case ResponseCode.Ok:
                    MarkAsSuccess(message);
                    break;
                case ResponseCode.BadRequest:
                    MarkAsBadRequest(message);
                    break;
                case ResponseCode.UnAuthorized:
                    MarkAsUnauthorized(message);
                    break;
                case ResponseCode.Forbidden:
                    MarkAsForbidden(message);
                    break;
                case ResponseCode.NotFound:
                    MarkAsNotFound(message);
                    break;
                case ResponseCode.Error:
                    MarkAsError(message);
                    break;
                default:
                    break;
            }
        }

        #region Properties
        /// <summary>
        /// Gets or sets the unique identifier for the response.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets the response code.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the response is successful.
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Gets or sets the message associated with the response.
        /// </summary>
        public string? Message { get; set; }
        #endregion

        #region Response Status Methods
        /// <summary>
        /// Marks the response as a successful operation.
        /// </summary>
        /// <param name="message">Optional message for success.</param>
        protected void MarkAsSuccess(string? message)
        {
            Code = (int)ResponseCode.Ok;
            Succeeded = true;
            Message = message ?? "Success";
        }

        /// <summary>
        /// Marks the response as a bad request.
        /// </summary>
        /// <param name="message">Optional message for bad request.</param>
        protected void MarkAsBadRequest(string? message)
        {
            Code = (int)ResponseCode.BadRequest;
            Succeeded = false;
            Message = message ?? "Bad Request";
        }

        /// <summary>
        /// Marks the response as unauthorized.
        /// </summary>
        /// <param name="message">Optional message for unauthorized access.</param>
        protected void MarkAsUnauthorized(string? message)
        {
            Code = (int)ResponseCode.UnAuthorized;
            Succeeded = false;
            Message = message ?? "Unauthorized";
        }

        /// <summary>
        /// Marks the response as forbidden.
        /// </summary>
        /// <param name="message">Optional message for forbidden access.</param>
        protected void MarkAsForbidden(string? message)
        {
            Code = (int)ResponseCode.Forbidden;
            Succeeded = false;
            Message = message ?? "Forbidden";
        }

        /// <summary>
        /// Marks the response as not found.
        /// </summary>
        /// <param name="message">Optional message for not found.</param>
        protected void MarkAsNotFound(string? message)
        {
            Code = (int)ResponseCode.NotFound;
            Succeeded = false;
            Message = message ?? "Not Found";
        }

        /// <summary>
        /// Marks the response as an error.
        /// </summary>
        /// <param name="message">Optional message for error.</param>
        protected void MarkAsError(string? message)
        {
            Code = (int)ResponseCode.Error;
            Succeeded = false;
            Message = message ?? "Error";
        }
        #endregion
    }
}