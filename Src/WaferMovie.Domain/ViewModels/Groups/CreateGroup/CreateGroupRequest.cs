namespace WaferMovie.Domain.ViewModels.Groups.CreateGroup;

public class CreateGroupRequest : IRequest<ApiResponse<Guid>>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public bool IsPublic { get; set; }
}
