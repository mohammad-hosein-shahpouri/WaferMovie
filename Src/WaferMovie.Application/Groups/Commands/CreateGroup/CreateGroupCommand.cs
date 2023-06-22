namespace WaferMovie.Application.Groups.Commands.CreateGroup;

public record CreateGroupCommand : GroupCoreModel, IRequest<CrudResult>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, CrudResult>
{
    private readonly IApplicationDbContext dbContext;

    public CreateGroupCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Group>();

        await dbContext.Groups.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}