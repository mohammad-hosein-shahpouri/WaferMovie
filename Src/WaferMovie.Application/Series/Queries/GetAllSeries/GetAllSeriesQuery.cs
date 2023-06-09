namespace WaferMovie.Application.Series.Queries.GetAllSeries;

public record GetAllSeriesQuery : IRequest<CrudResult<List<GetAllSeriesQueryDto>>>;

public class GetAllSeriesQueryHandler : IRequestHandler<GetAllSeriesQuery, CrudResult<List<GetAllSeriesQueryDto>>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IDatabase cacheDb;

    public GetAllSeriesQueryHandler(IApplicationDbContext dbContext, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<List<GetAllSeriesQueryDto>>> Handle(GetAllSeriesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "WaferMovie:Series:All";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<List<GetAllSeriesQueryDto>>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<List<GetAllSeriesQueryDto>>(cacheResult!)!);

        var series = await dbContext.Series.Include(i => i.Rates).Include(i => i.Seasons)
            .ThenInclude(ti => ti.Episodes).ThenInclude(ti => ti.DownloadLinks)
            .ProjectToType<GetAllSeriesQueryDto>().ToListAsync(cancellationToken);

        var cacheValue = JsonConvert.SerializeObject(series);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<List<GetAllSeriesQueryDto>>(CrudStatus.Succeeded, series);
    }
}