namespace WaferMovie.Domain.ViewModels.Movies.CreateMovie;

public record CreateMovieRequest : IRequest<ApiResponse<Guid>>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool Unavailable { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int OutYear { get; set; }
    public string IMDB { get; set; } = default!;
    public EnumMovieAgeRestriction AgeRestriction { get; set; }
}
