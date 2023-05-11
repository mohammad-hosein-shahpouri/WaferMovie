using WaferMovie.Application.Accounts.Queries;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
public class AccountsController : ControllerBase
{
    private readonly IMediator mediator;

    public AccountsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CrudResult<GetCurrentUserQueryDto>>> GetCurrentUser(CancellationToken cancellationToken)
        => await mediator.Send(new GetCurrentUserQuery(), cancellationToken);
}