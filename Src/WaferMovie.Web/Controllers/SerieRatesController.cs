using WaferSerie.Application.SerieRates.Commands.CreateSerieRate;

[Route("api/v{version}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
public class SerieRatesController : ControllerBase
{
    private readonly IMediator mediator;

    public SerieRatesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<CrudResult> CreateRate(CreateSerieRateCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);
}