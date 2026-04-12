namespace WaferMovie.Domain.ViewModels.Series.UpdateSerie;

public record UpdateSerieRequest : IRequest<ApiResponse<Guid>>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public EnumSerieAgeRestriction AgeRestriction { get; set; }
    public bool Unavailable { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int FirstSeasonYear { get; set; }
    public int? LastSeasonYear { get; set; }
}
