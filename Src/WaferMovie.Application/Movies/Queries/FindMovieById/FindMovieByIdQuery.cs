namespace WaferMovie.Application.Movies.Queries.FindMovieById;

public record FindMovieByIdQuery(int Id) : IRequest<CrudResult<FindMovieByIdQueryDto>>;

public class FindMovieByIdQueryHandler : IRequestHandler<FindMovieByIdQuery, CrudResult<FindMovieByIdQueryDto>>
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

    public async Task<CrudResult<FindMovieByIdQueryDto>> Handle(FindMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"WaferMovie:Movies:{request.Id}";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<FindMovieByIdQueryDto>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<FindMovieByIdQueryDto>(cacheResult!)!);

        var movie = await dbContext.Movies.Include(i => i.Rates).ProjectToType<FindMovieByIdQueryDto>()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (movie == null)
            return new CrudResult<FindMovieByIdQueryDto>(CrudStatus.NotFound, localization.FromValidationResources("No {0} found with this {1}", localization.FromPropertyResources("movie"), localization.FromPropertyResources(nameof(request.Id))));

        var cacheValue = JsonConvert.SerializeObject(movie);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<FindMovieByIdQueryDto>(CrudStatus.Succeeded, movie);
    }
}