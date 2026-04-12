namespace WaferMovie.Domain.ViewModels.Users.UpdateUser;

public record UpdateUserRequest : IRequest<ApiResponse<Guid>>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string UserName { get; set; } = default!;
}
