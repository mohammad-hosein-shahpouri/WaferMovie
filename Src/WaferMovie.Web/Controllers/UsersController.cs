using WaferMovie.Application.Users.Commands.CreateUser;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]")]
[ApiController, ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("Create")]
    public async Task<CrudResult<User>> Create(CreateUserCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);
}