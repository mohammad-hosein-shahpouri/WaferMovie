namespace WaferMovie.Application.Common.CoreModels;

public record GroupCoreModel
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public bool IsPublic { get; set; }
}