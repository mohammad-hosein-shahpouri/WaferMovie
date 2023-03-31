using WaferMovie.Application.Series.Commands.CreateSerie;

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

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateSerieCommand command)
    {
        var result = await mediator.Send(command);
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}