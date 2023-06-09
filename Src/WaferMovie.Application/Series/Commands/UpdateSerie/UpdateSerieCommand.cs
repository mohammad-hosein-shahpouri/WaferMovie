namespace WaferMovie.Application.Series.Commands.UpdateSerie;

public record UpdateSerieCommand : SerieCoreModel, IRequest<CrudResult<int>>
{
    public int Id { get; set; }
    public SerieAgeRestriction AgeRestriction { get; set; }
}

public class UpdateSerieCommandHandler : IRequestHandler<UpdateSerieCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;

    public UpdateSerieCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<int>> Handle(UpdateSerieCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Series.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new CrudResult<int>(CrudStatus.NotFound);

        entity = request.Adapt(entity);
        dbContext.Series.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}