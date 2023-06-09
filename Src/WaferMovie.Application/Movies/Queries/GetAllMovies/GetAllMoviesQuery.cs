namespace WaferMovie.Application.Movies.Queries.GetAllMovies;

public record GetAllMoviesQuery : IRequest<CrudResult<List<GetAllMoviesQueryDto>>>;

public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, CrudResult<List<GetAllMoviesQueryDto>>>
{
    private readonly IApplicationDbContext dbContext;

    public GetAllMoviesQueryHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<List<GetAllMoviesQueryDto>>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await dbContext.Movies
            .Include(i => i.DownloadLinks).Include(i => i.Rates)
            .ProjectToType<GetAllMoviesQueryDto>()
            .ToListAsync(cancellationToken);

        return new CrudResult<List<GetAllMoviesQueryDto>>(CrudStatus.Succeeded, movies);
    }
}