namespace WaferMovie.Application.Series.Queries.GetAllSeries;

public record GetAllSeriesQuery : IRequest<CrudResult<List<GetAllSeriesQueryDto>>>;

public class GetAllSeriesQueryHandler : IRequestHandler<GetAllSeriesQuery, CrudResult<List<GetAllSeriesQueryDto>>>
{
    private readonly IApplicationDbContext dbContext;

    public GetAllSeriesQueryHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<List<GetAllSeriesQueryDto>>> Handle(GetAllSeriesQuery request, CancellationToken cancellationToken)
    {
        var series = await dbContext.Series.Include(i => i.Rates).Include(i => i.Seasons)
            .ThenInclude(ti => ti.Episodes).ThenInclude(ti => ti.DownloadLinks)
            .ProjectToType<GetAllSeriesQueryDto>().ToListAsync(cancellationToken);

        return new CrudResult<List<GetAllSeriesQueryDto>>(CrudStatus.Succeeded, series);
    }
}