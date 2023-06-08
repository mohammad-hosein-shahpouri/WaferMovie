namespace WaferMovie.Application.Groups.Commands.CreateGroup;

public record CreateGroupCommand : IRequest<CrudResult>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public bool IsPublic { get; set; }
}

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