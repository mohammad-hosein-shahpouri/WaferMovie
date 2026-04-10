using WaferMovie.Application.MovieRates.Commands.CreateMovieRate;
using WaferMovie.Application.Movies.Commands.CreateMovie;
using WaferMovie.Application.Movies.Commands.DeleteMovie;
using WaferMovie.Application.Movies.Commands.UpdateMovie;
using WaferMovie.Application.Movies.Queries.FindMovieById;
using WaferMovie.Application.Movies.Queries.GetAllMovies;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController, ApiVersion("1.0")]
public class MoviesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ApiResponse<List<GetAllMoviesQueryDto>>> GetAll(CancellationToken cancellationToken)
        => await mediator.Send(new GetAllMoviesQuery(), cancellationToken);

    [HttpGet("{id}")]
    public async Task<ApiResponse<FindMovieByIdQueryDto>> FindById(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new FindMovieByIdQuery(id), cancellationToken);


    [HttpPost]
    public async Task<ApiResponse<Guid>> CreateMovie(CreateMovieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ApiResponse<Guid>> UpdateMovie(Guid id, UpdateMovieCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command with { Id = id }, cancellationToken);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<Guid>> DeleteMovie(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteMovieCommand(id), cancellationToken);

    [HttpPost("{id}/Rate")]
    public async Task<ApiResponse> CreateRate(Guid id, CreateMovieRateCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command with { MovieId = id }, cancellationToken);
}