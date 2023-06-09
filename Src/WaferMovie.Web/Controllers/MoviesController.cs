using WaferMovie.Application.Movies.Commands.CreateMovie;
using WaferMovie.Application.Movies.Commands.DeleteMovie;
using WaferMovie.Application.Movies.Commands.UpdateMovie;
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
    public async Task<CrudResult<List<GetAllMoviesQueryDto>>> GetAll(CancellationToken cancellationToken)
        => await mediator.Send(new GetAllMoviesQuery(), cancellationToken);

    [HttpGet("FindById/{id}")]
    public async Task<CrudResult<FindMovieByIdQueryDto>> FindById(int id, CancellationToken cancellationToken)
        => await mediator.Send(new FindMovieByIdQuery(id), cancellationToken);

    #endregion Query

    #region Command

    [HttpPost("Create")]
    public async Task<CrudResult<int>> CreateMovie(CreateMovieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpPut("Update")]
    public async Task<CrudResult<int>> UpdateMovie(UpdateMovieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpDelete("Delete")]
    public async Task<CrudResult<int>> DeleteMovie(DeleteMovieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    #endregion Command
}