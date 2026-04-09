namespace WaferMovie.Domain.ViewModels.Accounts.Login;

public class LoginResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public int AccountBalance { get; set; }

    public required string Token { get; set; }
}
