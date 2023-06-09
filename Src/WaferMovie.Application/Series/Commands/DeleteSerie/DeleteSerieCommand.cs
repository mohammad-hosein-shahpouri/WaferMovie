namespace WaferMovie.Application.Series.Commands.DeleteSerie;

public record DeleteSerieCommand(int Id) : IRequest<CrudResult<int>>;

public class DeleteSerieCommandHandler : IRequestHandler<DeleteSerieCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;

    public DeleteSerieCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<int>> Handle(DeleteSerieCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Series.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new CrudResult<int>(CrudStatus.NotFound);

        dbContext.Series.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}