namespace WaferMovie.Application.Groups.Commands.DeleteGroup;

public record DeleteGroupCommand(int Id) : IRequest<CrudResult>;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, CrudResult>
{
    private readonly IApplicationDbContext dbContext;

    public DeleteGroupCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Groups.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (group == null) return new CrudResult(CrudStatus.NotFound);

        group.IsDeleted = true;
        dbContext.Groups.Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CrudResult(CrudStatus.Succeeded);
    }
}