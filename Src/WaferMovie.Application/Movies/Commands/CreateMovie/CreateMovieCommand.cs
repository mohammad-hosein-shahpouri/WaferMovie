using StackExchange.Redis;

namespace WaferMovie.Application.Movies.Commands.CreateMovie;

[ValidateNever]
public record CreateMovieCommand : MovieCoreModel, IRequest<CrudResult<int>>
{
    public string IMDB { get; set; } = default!;
    public MovieAgeRestriction AgeRestriction { get; set; }
}

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IDatabase cacheDb;

    public CreateMovieCommandHandler(IApplicationDbContext dbContext, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<int>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Movie>();

        await dbContext.Movies.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheDb.KeyDeleteAsync("WaferMovie:Movies:All");

        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}