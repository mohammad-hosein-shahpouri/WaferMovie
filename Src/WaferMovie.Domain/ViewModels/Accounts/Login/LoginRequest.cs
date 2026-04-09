namespace WaferMovie.Domain.ViewModels.Accounts.Login;

public class LoginRequest : IRequest<ApiResponse<LoginResponse>>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsPersistent { get; set; }
}
