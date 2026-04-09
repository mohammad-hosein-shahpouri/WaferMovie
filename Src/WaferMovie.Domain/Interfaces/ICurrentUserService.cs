namespace WaferMovie.Domain.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    Guid Id { get; }
}