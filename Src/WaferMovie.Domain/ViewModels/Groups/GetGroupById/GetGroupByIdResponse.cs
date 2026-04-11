namespace WaferMovie.Domain.ViewModels.Groups.GetGroupById;

public class GetGroupByIdResponse : IRegister
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Group, GetGroupByIdResponse>();
    }
}