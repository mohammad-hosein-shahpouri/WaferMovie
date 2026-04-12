using WaferMovie.Domain.ViewModels.Series.CreateSerie;
using WaferMovie.Domain.ViewModels.Series.CreateSerieRate;
using WaferMovie.Domain.ViewModels.Series.DeleteSerie;
using WaferMovie.Domain.ViewModels.Series.GetSerieById;
using WaferMovie.Domain.ViewModels.Series.UpdateSerie;

namespace WaferMovie.Web.Controllers;

[ApiController, ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SeriesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ApiResponse<GetSerieByIdResponse>> FindById(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new GetSerieByIdRequest { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<Guid>> CreateSerie(CreateSerieRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ApiResponse<Guid>> UpdateSerie(Guid id, UpdateSerieRequest request, CancellationToken cancellationToken)
     => await mediator.Send(request with { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteSerie(Guid id, CancellationToken cancellationToken)
     => await mediator.Send(new DeleteSerieRequest { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{id}/Rate")]
    public async Task<ApiResponse> CreateRate(Guid id, CreateSerieRateRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request with { SerieId = id }, cancellationToken);
}