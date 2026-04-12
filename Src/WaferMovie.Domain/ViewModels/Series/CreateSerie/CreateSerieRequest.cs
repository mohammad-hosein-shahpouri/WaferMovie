namespace WaferMovie.Domain.ViewModels.Series.CreateSerie;

public record CreateSerieRequest : IRequest<ApiResponse<Guid>>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string IMDB { get; set; } = default!;
    public EnumSerieAgeRestriction AgeRestriction { get; set; }
    public bool Unavailable { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int FirstSeasonYear { get; set; }
    public int? LastSeasonYear { get; set; }
}
