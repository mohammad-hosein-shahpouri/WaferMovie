using WaferMovie.Application.Movies.Commands.CreateMovieCommand;

namespace WaferMovie.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMediator mediator;

    public MoviesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateMovie(CreateMovieCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}