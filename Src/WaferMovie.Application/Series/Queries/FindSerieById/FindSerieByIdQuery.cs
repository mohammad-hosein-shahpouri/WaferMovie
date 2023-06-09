namespace WaferMovie.Application.Series.Queries.FindSerieById;

public record FindSerieByIdQuery(int Id) : IRequest<CrudResult<FindSerieByIdQueryDto>>;

public class FindSerieByIdQueryHandler : IRequestHandler<FindSerieByIdQuery, CrudResult<FindSerieByIdQueryDto>>
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

    public async Task<CrudResult<FindSerieByIdQueryDto>> Handle(FindSerieByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"WaferMovie:Series:{request.Id}";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<FindSerieByIdQueryDto>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<FindSerieByIdQueryDto>(cacheResult!)!);

        var serie = await dbContext.Series.ProjectToType<FindSerieByIdQueryDto>()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (serie == null)
            return new CrudResult<FindSerieByIdQueryDto>(CrudStatus.NotFound, localization.FromValidationResources("No {0} found with this {1}", localization.FromPropertyResources("serie"), localization.FromPropertyResources(nameof(request.Id))));

        var cacheValue = JsonConvert.SerializeObject(serie);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<FindSerieByIdQueryDto>(CrudStatus.Succeeded, serie);
    }
}