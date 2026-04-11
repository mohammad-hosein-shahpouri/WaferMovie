namespace WaferMovie.Domain.ViewModels.Groups.UpdateGroup;

public record UpdateGroupRequest : IRequest<ApiResponse<Guid>>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public bool IsPublic { get; set; }
}
