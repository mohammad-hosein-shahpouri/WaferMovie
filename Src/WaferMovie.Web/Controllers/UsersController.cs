using WaferMovie.Application.Users.Commands.CreateUser;
using WaferMovie.Application.Users.Commands.UpdateUser;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]/[action]")]
[ApiController, ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region Commands

    [HttpPost]
    public async Task<CrudResult<User>> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpPut]
    public async Task<CrudResult<int>> UpdateUser(UpdateUserCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    #endregion Commands
}