namespace WaferMovie.Application.Common.CoreModels;

public record UserCoreModel
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string UserName { get; set; } = default!;
}