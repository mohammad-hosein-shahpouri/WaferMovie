using Newtonsoft.Json;
using StackExchange.Redis;

namespace WaferMovie.Application.Series.Queries.GetAllSeries;

public record GetAllSeriesQuery : IRequest<CrudResult<List<Serie>>>;

public class GetAllSeriesQueryHandler : IRequestHandler<GetAllSeriesQuery, CrudResult<List<Serie>>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IDatabase cacheDb;

    public GetAllSeriesQueryHandler(IApplicationDbContext dbContext, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<List<Serie>>> Handle(GetAllSeriesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "Series:All";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<List<Serie>>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<List<Serie>>(cacheResult!)!);

        var series = await dbContext.Series.Include(i => i.Seasons)
            .ThenInclude(ti => ti.Episodes).ThenInclude(ti => ti.DownloadLinks).ToListAsync();

        var cacheValue = JsonConvert.SerializeObject(series);
        await cacheDb.StringSetAsync(cacheKey, cacheValue);

        return new CrudResult<List<Serie>>(CrudStatus.Succeeded, series);
    }
}