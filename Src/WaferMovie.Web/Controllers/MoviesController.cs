using WaferMovie.Application.Movies.Commands.CreateMovie;
using WaferMovie.Application.Movies.Queries.FindMovieById;
using WaferMovie.Application.Movies.Queries.GetAllMovies;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class MoviesController : ControllerBase
{
    private readonly IMediator mediator;

    public MoviesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region Query

    [HttpGet("All")]
    public async Task<ActionResult<CrudResult<List<Movie>>>> GetAll(CancellationToken cancellationToken)
        => await mediator.Send(new GetAllMoviesQuery(), cancellationToken);

    [HttpGet("FindById/{id}")]
    public async Task<ActionResult<CrudResult<Movie>>> FindById(int id, CancellationToken cancellationToken)
        => await mediator.Send(new FindMovieByIdQuery(id), cancellationToken);

    #endregion Query

    #region Command

    [HttpPost("Create")]
    public async Task<ActionResult<CrudResult<Movie>>> CreateMovie(CreateMovieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    #endregion Command
}