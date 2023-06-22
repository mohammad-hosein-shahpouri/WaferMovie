namespace WaferMovie.Application.MovieRates.Commands.CreateMovieRate;

public record CreateMovieRateCommand(int MovieId, byte Score) : IRequest<CrudResult>;

public class CreateMovieRateCommandHandler : IRequestHandler<CreateMovieRateCommand, CrudResult>
{
    private readonly IApplicationDbContext dbContext;
    private readonly ICurrentUserService currentUserService;

    public CreateMovieRateCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        this.dbContext = dbContext;
        this.currentUserService = currentUserService;
    }

    public async Task<CrudResult> Handle(CreateMovieRateCommand request, CancellationToken cancellationToken)
    {
        var entity = new MovieRate
        {
            MovieId = request.MovieId,
            Score = request.Score,
            UserId = currentUserService.Id
        };

        dbContext.MovieRates.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}