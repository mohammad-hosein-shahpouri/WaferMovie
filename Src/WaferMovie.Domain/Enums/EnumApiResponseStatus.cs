namespace WaferMovie.Domain.Enums;

public enum EnumApiResponseStatus
{
    Success = 200,
    Failure = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InvalidData = 406,
    Error = 500
}
