using WaferMovie.Application.Movies.Queries.FindMovieById;
using WaferMovie.Application.Series.Commands.CreateSerie;
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
    public async Task<CrudResult<List<Serie>>> GetAll(CancellationToken cancellationToken)
        => await mediator.Send(new GetAllSeriesQuery(), cancellationToken);

    [HttpGet("FindById/{id}")]
    public async Task<CrudResult<Serie>> FindById(int id, CancellationToken cancellationToken)
        => await mediator.Send(new FindSerieByIdQuery(id), cancellationToken);

    #endregion Query

    #region Command

    [HttpPost("Create")]
    public async Task<CrudResult<Serie>> Create(CreateSerieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    #endregion Command
}