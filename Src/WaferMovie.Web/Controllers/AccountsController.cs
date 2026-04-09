using WaferMovie.Domain.ViewModels.Accounts.GetCurrentUser;
using WaferMovie.Domain.ViewModels.Accounts.Login;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]")]
[ApiController, ApiVersion("1.0")]
[Authorize]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ApiResponse<GetCurrentUserResponse>> GetCurrentUser(CancellationToken cancellationToken)
        => await mediator.Send(new GetCurrentUserRequest(), cancellationToken);

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);
}