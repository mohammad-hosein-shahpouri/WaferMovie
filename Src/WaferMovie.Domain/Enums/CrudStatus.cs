namespace WaferMovie.Domain.Enums;

public enum CrudStatus
{
    Succeeded = 1,
    Failed = 0,
    ValidationError = -1,
    NotFound = -2,
}