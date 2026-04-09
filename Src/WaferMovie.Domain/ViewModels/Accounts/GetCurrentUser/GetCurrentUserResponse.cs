namespace WaferMovie.Domain.ViewModels.Accounts.GetCurrentUser;

public class GetCurrentUserResponse : IRegister
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int AccountBalance { get; set; }

    public void Register(TypeAdapterConfig config)
    {
    }
}