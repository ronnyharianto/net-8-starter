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
        /// <param name="httpStatusCode">The HTTP status code indicating the response result.</param>
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

        /// <summary>
        /// Unique identifier representing the entire process lifecycle,
        /// from the incoming request to the outgoing response.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The HTTP status code of the response.
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

        /// <summary>
        /// Marks the response as a successful operation.
        /// </summary>
        /// <param name="message">Optional success message.</param>
        protected void MarkAsSuccess(string? message)
        {
            Code = (int)HttpStatusCode.OK;
            Succeeded = true;
            Message = message ?? "OK";
        }

        /// <summary>
        /// Marks the response as a non-successful operation.
        /// </summary>
        /// <param name="message">Optional failure message.</param>
        /// <param name="httpStatusCode">The HTTP status code indicating failure reason.</param>
        protected void MarkAsNotSuccess(string? message, HttpStatusCode httpStatusCode)
        {
            Code = (int)httpStatusCode;
            Succeeded = false;
            Message = message ?? "Not OK";
        }
    }
}