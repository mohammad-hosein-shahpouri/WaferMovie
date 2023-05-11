using StackExchange.Redis;

namespace WaferMovie.Application.Series.Queries.FindSerieById;

public record FindSerieByIdQuery(int Id) : IRequest<CrudResult<Serie>>;

public class FindSerieByIdQueryHandler : IRequestHandler<FindSerieByIdQuery, CrudResult<Serie>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly ILocalizationService localization;
    private readonly IDatabase cacheDb;

    public FindSerieByIdQueryHandler(IApplicationDbContext dbContext, ILocalizationService localization, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.localization = localization;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<Serie>> Handle(FindSerieByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"WaferMovie:Series:{request.Id}";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<Serie>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<Serie>(cacheResult!)!);

        var serie = await dbContext.Series.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (serie == null)
            return new CrudResult<Serie>(CrudStatus.NotFound, string.Format(localization.FromValidationResources("No {0} found with this {1}"), localization.FromPropertyResources("serie"), localization.FromPropertyResources(nameof(request.Id))));

        var cacheValue = JsonConvert.SerializeObject(serie);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<Serie>(CrudStatus.Succeeded, serie);
    }
}