using WaferMovie.Application.MovieRates.Commands.CreateMovieRate;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
public class MovieRatesController : ControllerBase
{
    private readonly IMediator mediator;

    public MovieRatesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<CrudResult> CreateRate(CreateMovieRateCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);
}