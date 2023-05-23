using Microsoft.AspNetCore.Authorization;
using WaferMovie.Application.Accounts.Commands.Login;
using WaferMovie.Application.Accounts.Queries.GetCurrentUser;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]")]
[ApiController, ApiVersion("1.0")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IMediator mediator;

    public AccountsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<CrudResult<GetCurrentUserQueryDto>> GetCurrentUser(CancellationToken cancellationToken)
        => await mediator.Send(new GetCurrentUserQuery(), cancellationToken);

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<CrudResult> Login(LoginCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);
}