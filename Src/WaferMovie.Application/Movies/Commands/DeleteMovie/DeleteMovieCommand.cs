namespace WaferMovie.Application.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand(int Id) : IRequest<CrudResult<int>>;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;

    public DeleteMovieCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<int>> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Movies.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new CrudResult<int>(CrudStatus.NotFound);

        dbContext.Movies.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}