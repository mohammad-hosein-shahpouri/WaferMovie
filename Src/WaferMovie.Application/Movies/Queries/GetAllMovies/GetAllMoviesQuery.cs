using StackExchange.Redis;

namespace WaferMovie.Application.Movies.Queries.GetAllMovies;

public record GetAllMoviesQuery : IRequest<CrudResult<List<GetAllMoviesQueryDto>>>;

public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, CrudResult<List<GetAllMoviesQueryDto>>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IDatabase cacheDb;

    public GetAllMoviesQueryHandler(IApplicationDbContext dbContext, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<List<GetAllMoviesQueryDto>>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "WaferMovie:Movies:All";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<List<GetAllMoviesQueryDto>>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<List<GetAllMoviesQueryDto>>(cacheResult!)!);

        var movies = await dbContext.Movies
            .Include(i => i.DownloadLinks).Include(i => i.Rates)
            .ProjectToType<GetAllMoviesQueryDto>()
            .ToListAsync(cancellationToken);

        var cacheValue = JsonConvert.SerializeObject(movies);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<List<GetAllMoviesQueryDto>>(CrudStatus.Succeeded, movies);
    }
}