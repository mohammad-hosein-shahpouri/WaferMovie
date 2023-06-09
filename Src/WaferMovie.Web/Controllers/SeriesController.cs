using WaferMovie.Application.Series.Commands.CreateSerie;
using WaferMovie.Application.Series.Commands.DeleteSerie;
using WaferMovie.Application.Series.Commands.UpdateSerie;
using WaferMovie.Application.Series.Queries.FindSerieById;
using WaferMovie.Application.Series.Queries.GetAllSeries;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class SeriesController : ControllerBase
{
    private readonly IMediator mediator;

    public SeriesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region Query

    [HttpGet("All")]
    public async Task<CrudResult<List<GetAllSeriesQueryDto>>> GetAll(CancellationToken cancellationToken)
        => await mediator.Send(new GetAllSeriesQuery(), cancellationToken);

    [HttpGet("FindById/{id}")]
    public async Task<CrudResult<FindSerieByIdQueryDto>> FindById(int id, CancellationToken cancellationToken)
        => await mediator.Send(new FindSerieByIdQuery(id), cancellationToken);

    #endregion Query

    #region Command

    [HttpPost("Create")]
    public async Task<CrudResult<int>> CreateSerie(CreateSerieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpPut("Update")]
    public async Task<CrudResult<int>> UpdateSerie(UpdateSerieCommand command, CancellationToken cancellationToken)
     => await mediator.Send(command, cancellationToken);

    [HttpDelete("Delete")]
    public async Task<CrudResult<int>> DeleteSerie(DeleteSerieCommand command, CancellationToken cancellationToken)
     => await mediator.Send(command, cancellationToken);

    #endregion Command
}