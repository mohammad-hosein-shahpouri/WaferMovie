using StackExchange.Redis;

namespace WaferMovie.Application.Movies.Queries.FindMovieById;

public record FindMovieByIdQuery(int Id) : IRequest<CrudResult<Movie>>;

public class FindMovieByIdQueryHandler : IRequestHandler<FindMovieByIdQuery, CrudResult<Movie>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly ILocalizationService localization;
    private readonly IDatabase cacheDb;

    public FindMovieByIdQueryHandler(IApplicationDbContext dbContext, ILocalizationService localization, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.localization = localization;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<Movie>> Handle(FindMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Movies:{request.Id}";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<Movie>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<Movie>(cacheResult!)!);

        var movie = await dbContext.Movies.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (movie == null)
            return new CrudResult<Movie>(CrudStatus.NotFound, string.Format(localization.FromValidationResources("No {0} found with this {1}"), localization.FromPropertyResources("movie"), localization.FromPropertyResources(nameof(request.Id))));

        var cacheValue = JsonConvert.SerializeObject(movie);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<Movie>(CrudStatus.Succeeded, movie);
    }
}