namespace WaferMovie.Application.Series.Queries.FindSerieById;

public class FindSerieByIdQueryDto : IRegister
{
    public int Id { get; set; }
    public string IMDB { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public string? StreamNetwork { get; set; }
    public double AverageScore { get; set; }
    public int FirstSeasonYear { get; set; }
    public int? LastSeasonYear { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Serie, FindSerieByIdQueryDto>()
            .Map(dst => dst.AverageScore, src => src.Rates.Average(a => a.Score),
                condition => condition.Rates.Any());
    }
}