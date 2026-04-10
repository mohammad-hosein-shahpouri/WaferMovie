using WaferMovie.Application.Series.Commands.CreateSerie;
using WaferMovie.Application.Series.Commands.DeleteSerie;
using WaferMovie.Application.Series.Commands.UpdateSerie;
using WaferMovie.Application.Series.Queries.FindSerieById;
using WaferMovie.Application.Series.Queries.GetAllSeries;
using WaferSerie.Application.SerieRates.Commands.CreateSerieRate;

namespace WaferMovie.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SeriesController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<ApiResponse<List<GetAllSeriesQueryDto>>> GetAll(CancellationToken cancellationToken)
        => await mediator.Send(new GetAllSeriesQuery(), cancellationToken);

    [HttpGet("{id}")]
    public async Task<ApiResponse<FindSerieByIdQueryDto>> FindById(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new FindSerieByIdQuery(id), cancellationToken);

    [HttpPost]
    public async Task<ApiResponse<Guid>> CreateSerie(CreateSerieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ApiResponse<Guid>> UpdateSerie(Guid id, UpdateSerieCommand command, CancellationToken cancellationToken)
     => await mediator.Send(command with { Id = id }, cancellationToken);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<Guid>> DeleteSerie(Guid id, CancellationToken cancellationToken)
     => await mediator.Send(new DeleteSerieCommand(id), cancellationToken);

    [HttpPost("{id}/Rate")]
    public async Task<ApiResponse> CreateRate(Guid id, CreateSerieRateCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command with { SerieId = id }, cancellationToken);
}