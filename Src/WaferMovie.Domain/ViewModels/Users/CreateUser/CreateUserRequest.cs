namespace WaferMovie.Domain.ViewModels.Users.CreateUser;

public record CreateUserRequest : IRequest<ApiResponse<Guid>>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
    public string PasswordConfirmation { get; set; } = default!;
}
