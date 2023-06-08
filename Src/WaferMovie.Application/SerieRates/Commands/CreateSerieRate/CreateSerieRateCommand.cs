namespace WaferSerie.Application.SerieRates.Commands.CreateSerieRate;

public record CreateSerieRateCommand(int SerieId, byte Score) : IRequest<CrudResult>;

public class CreateSerieRateCommandHandler : IRequestHandler<CreateSerieRateCommand, CrudResult>
{
    private readonly IApplicationDbContext dbContext;
    private readonly ICurrentUserService currentUserService;

    public CreateSerieRateCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        this.dbContext = dbContext;
        this.currentUserService = currentUserService;
    }

    public async Task<CrudResult> Handle(CreateSerieRateCommand request, CancellationToken cancellationToken)
    {
        var entity = new SerieRate
        {
            SerieId = request.SerieId,
            Score = request.Score,
            UserId = currentUserService.Id!.Value
        };

        dbContext.SerieRates.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}