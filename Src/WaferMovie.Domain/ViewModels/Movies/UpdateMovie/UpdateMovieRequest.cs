namespace WaferMovie.Domain.ViewModels.Movies.UpdateMovie;

public record UpdateMovieRequest : IRequest<ApiResponse<Guid>>
{
    [JsonIgnore]
    public required Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool Unavailable { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int OutYear { get; set; }
    public EnumMovieAgeRestriction AgeRestriction { get; set; }
}
