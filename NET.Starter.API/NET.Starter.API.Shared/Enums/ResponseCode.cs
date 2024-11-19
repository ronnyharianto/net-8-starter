namespace NET.Starter.API.Shared.Enums
{
    internal enum ResponseCode
    {
        Ok = 200,
        BadRequest = 400,
        UnAuthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        TimeOut = 408,
        Error = 500
    }
}
