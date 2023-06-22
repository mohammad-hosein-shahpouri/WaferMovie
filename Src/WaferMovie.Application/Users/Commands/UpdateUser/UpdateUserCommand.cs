namespace WaferMovie.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(int Id) : UserCoreModel, IRequest<CrudResult<int>>;

internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;

    public UpdateUserCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<int>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (user == null) return new CrudResult<int>(CrudStatus.NotFound);

        request.Adapt(user);
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CrudResult<int>(CrudStatus.Succeeded, user.Id);
    }
}