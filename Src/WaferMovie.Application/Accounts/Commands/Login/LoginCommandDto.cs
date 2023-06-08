namespace WaferMovie.Application.Accounts.Commands.Login;

public class LoginCommandDto : IRegister
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int AccountBalance { get; set; }

    public string Token { get; set; } = default!;

    public void Register(TypeAdapterConfig config)
    {
    }
}