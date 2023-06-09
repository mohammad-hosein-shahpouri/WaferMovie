namespace WaferMovie.Application.Movies.Queries.FindMovieById;

public class FindMovieByIdQueryDto : IRegister
{
    public int Id { get; set; }
    public string IMDB { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double AverageScore { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int OutYear { get; set; }
    public string AgeRestriction { get; set; } = default!;

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Movie, FindMovieByIdQueryDto>()
        .Map(dst => dst.Id, src => src.Id)
        .Map(dst => dst.IMDB, src => src.IMDB)
        .Map(dst => dst.Title, src => src.Title)
        .Map(dst => dst.Description, src => src.Description)
        .Map(dst => dst.AverageScore, src => src.Rates.Average(x => x.Score),
            condition => condition.Rates.Any())
        .Map(dst => dst.Length, src => src.Length)
        .Map(dst => dst.IsFree, src => src.IsFree)
        .Map(dst => dst.OutYear, src => src.OutYear)
        .Map(dst => dst.AgeRestriction, src => src.AgeRestriction.ToString())

            ;
    }
}