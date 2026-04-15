using WaferMovie.Domain.Common.Pagination;
using WaferMovie.Domain.ViewModels.Movies.GetMoviesPaginated;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController, ApiVersion("1.0")]
public class SearchController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// This method searches in movies and returns in a ordered list
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<PaginationOutput<GetMoviesPaginatedResponse>>> GetAllPaginated(GetMoviesPaginatedRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request, cancellationToken);

    [HttpPost("Movies")]
    public async Task<ApiResponse<PaginationOutput<GetMoviesPaginatedResponse>>> GetMoviesPaginated(GetMoviesPaginatedRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request, cancellationToken);

    [HttpPost("Series")]
    public async Task<ApiResponse<PaginationOutput<GetMoviesPaginatedResponse>>> GetSeriesPaginated(GetMoviesPaginatedRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request, cancellationToken);
}
