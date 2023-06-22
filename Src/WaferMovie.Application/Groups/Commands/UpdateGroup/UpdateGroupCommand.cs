namespace WaferMovie.Application.Groups.Commands.UpdateGroup;

public record UpdateGroupCommand(int Id) : GroupCoreModel, IRequest<CrudResult<int>>;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;

    public UpdateGroupCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<int>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Groups.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (group == null) return new CrudResult<int>(CrudStatus.NotFound);

        request.Adapt(group);
        dbContext.Groups.Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CrudResult<int>(CrudStatus.Succeeded, group.Id);
    }
}