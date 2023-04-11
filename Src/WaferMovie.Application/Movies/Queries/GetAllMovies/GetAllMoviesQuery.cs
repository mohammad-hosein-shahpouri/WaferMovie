using Newtonsoft.Json;
using StackExchange.Redis;

namespace WaferMovie.Application.Movies.Queries.GetAllMovies;

public record GetAllMoviesQuery : IRequest<CrudResult<List<Movie>>>;

public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, CrudResult<List<Movie>>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IDatabase cacheDb;

    public GetAllMoviesQueryHandler(IApplicationDbContext dbContext, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<List<Movie>>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "Movies:All";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<List<Movie>>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<List<Movie>>(cacheResult!)!);

        var movies = await dbContext.Movies.Include(i => i.DownloadLinks).ToListAsync();
        var cacheValue = JsonConvert.SerializeObject(movies);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<List<Movie>>(CrudStatus.Succeeded, movies);
    }
}