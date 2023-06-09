namespace WaferMovie.Application.Movies.Commands.UpdateMovie;

public record UpdateMovieCommand : MovieCoreModel, IRequest<CrudResult<int>>
{
    public int Id { get; set; }
    public MovieAgeRestriction AgeRestriction { get; set; }
}

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IDatabase cacheDb;

    public UpdateMovieCommandHandler(IApplicationDbContext dbContext, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<int>> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Movies.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new CrudResult<int>(CrudStatus.NotFound);

        entity = request.Adapt(entity);
        dbContext.Movies.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cacheDb.KeyDeleteAsync("WaferMovie:Movies:All");
        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}