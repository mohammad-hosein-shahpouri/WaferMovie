namespace WaferMovie.Application.Groups.Queries.FindGroupById;

public class FindGroupByIdQueryDto : IRegister
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Group, FindGroupByIdQueryDto>();
    }
}