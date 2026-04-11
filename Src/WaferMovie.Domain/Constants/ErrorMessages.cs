namespace WaferMovie.Domain.Constants;

public static class ErrorMessages
{
    public const string IS_REQUIRED = "{0} is required";
    public const string IS_INVALID = "{0} is invalid";
    public const string IS_NOT_FOUND = "{0} is not found";

    public const string CAN_NOT_BE_LONGER_THAN = "{0} can not be longer than {{MaxLength}} characters";

    public const string MUST_BE_BETWEEN = "{0} must be between {{From}} and {{To}}";
}