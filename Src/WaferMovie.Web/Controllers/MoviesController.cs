using WaferMovie.Domain.ViewModels.Movies.CreateMovie;
using WaferMovie.Domain.ViewModels.Movies.CreateMovieRate;
using WaferMovie.Domain.ViewModels.Movies.DeleteMovie;
using WaferMovie.Domain.ViewModels.Movies.GetMovieById;
using WaferMovie.Domain.ViewModels.Movies.UpdateMovie;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController, ApiVersion("1.0")]
public class MoviesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ApiResponse<GetMovieByIdResponse>> GetMovieById(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new GetMovieByIdRequest { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<Guid>> CreateMovie(CreateMovieRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ApiResponse<Guid>> UpdateMovie(Guid id, UpdateMovieRequest command, CancellationToken cancellationToken)
        => await mediator.Send(command with { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteMovie(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteMovieRequest { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{id}/Rate")]
    public async Task<ApiResponse<Guid>> CreateRate(Guid id, CreateMovieRateRequest command, CancellationToken cancellationToken)
        => await mediator.Send(command with { MovieId = id }, cancellationToken);
}