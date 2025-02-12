namespace NET.Starter.Shared.Enums
{
    /// <summary>
    /// Represents standard HTTP response codes that can be used to indicate 
    /// the outcome of a request in an application.
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// Indicates that the request has succeeded.
        /// </summary>
        Ok = 200,

        /// <summary>
        /// Indicates that the server cannot process the request due to a client error.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// Indicates that the request requires user authentication, 
        /// but the client has failed to provide valid credentials.
        /// </summary>
        UnAuthorized = 401,

        /// <summary>
        /// Indicates that the client does not have access rights to the content.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// Indicates that the server cannot find the requested resource.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// Indicates that the server encountered an internal error and could not complete the request.
        /// </summary>
        Error = 500
    }
}