using System.Net;

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
        /// <param name="httpStatusCode">The response code indicating the result.</param>
        public BaseDto(string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    MarkAsSuccess(message);
                    break;
                default:
                    MarkAsNotSuccess(message, httpStatusCode);
                    break;
            }
        }

        #region Properties
        /// <summary>
        /// Unique identifier representing the entire process lifecycle, 
        /// from the incoming request to the outgoing response.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Response code.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Indicating whether the response is successful.
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Message associated with the response.
        /// </summary>
        public string? Message { get; set; }
        #endregion

        #region Response Status Methods
        /// <summary>
        /// Marks the response as a successful operation.
        /// </summary>
        protected void MarkAsSuccess(string? message)
        {
            Code = (int)HttpStatusCode.OK;
            Succeeded = true;
            Message = message ?? "OK";
        }

        /// <summary>
        /// Marks the response as a non-successful operation.
        /// </summary>
        protected void MarkAsNotSuccess(string? message, HttpStatusCode httpStatusCode)
        {
            Code = (int)httpStatusCode;
            Succeeded = false;
            Message = message ?? "Not OK";
        }
        #endregion
    }
}