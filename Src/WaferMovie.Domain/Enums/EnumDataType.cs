namespace WaferMovie.Domain.Enums;

public enum EnumDataType
{
    Number = 1,
    CommaSeparatedNumber,

    //Float,
    String,
    Guid,

    Date,
    Time,
    DateTime,
    Boolean,

    /// <summary>
    /// Use for dropdown
    /// </summary>
    Enum,

    Rate,
    Email,
    PhoneNumber
}