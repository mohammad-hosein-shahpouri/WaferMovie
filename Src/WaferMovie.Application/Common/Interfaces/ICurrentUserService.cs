namespace WaferMovie.Application.Common.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    int? Id { get; }
}