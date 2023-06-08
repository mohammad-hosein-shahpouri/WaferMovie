namespace WaferMovie.Application.Series.Queries.GetAllSeries;

public class GetAllSeriesQueryDto : IRegister
{
    public int Id { get; set; }
    public string IMDB { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public string? StreamNetwork { get; set; }
    public double AverageScore { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Serie, GetAllSeriesQueryDto>()
            .Map(dst => dst.Id, src => src.Id)
            .Map(dst => dst.IMDB, src => src.IMDB)
            .Map(dst => dst.Title, src => src.Title)
            .Map(dst => dst.Description, src => src.Description)
            .Map(dst => dst.Length, src => src.Length)
            .Map(dst => dst.IsFree, src => src.IsFree)
            .Map(dst => dst.StreamNetwork, src => src.StreamNetwork)
            .Map(dst => dst.AverageScore, src => src.Rates.Average(a => a.Score),
                condition => condition.Rates.Any());
    }
}