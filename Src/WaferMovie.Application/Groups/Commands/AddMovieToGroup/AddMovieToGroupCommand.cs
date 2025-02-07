namespace WaferMovie.Application.Groups.Commands.AddMovieToGroup;

public record AddMovieToGroupCommand(int MovieId, int GroupId) : IRequest<CrudResult>;

public class AddMovieToGroupCommandHandler : IRequestHandler<AddMovieToGroupCommand, CrudResult>
{
    private readonly IApplicationDbContext dbContext;

    public AddMovieToGroupCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult> Handle(AddMovieToGroupCommand request, CancellationToken cancellationToken)
    {
        var movieGroup = request.Adapt<MovieGroup>();
        await dbContext.MovieGroups.AddAsync(movieGroup!, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CrudResult(CrudStatus.Succeeded);
    }
}