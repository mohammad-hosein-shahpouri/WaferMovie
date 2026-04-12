namespace WaferMovie.Domain.ViewModels.Users.DeleteUser;

public record DeleteUserRequest : IRequest<ApiResponse>
{
    public required Guid Id { get; set; }
}
