using WaferMovie.Domain.ViewModels.Accounts.GetCurrentUser;
using WaferMovie.Domain.ViewModels.Accounts.Login;

namespace WaferMovie.Web.Controllers;

[Authorize]
[ApiController, ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AccountsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Returns information about current user
    /// </summary>
    /// <response code="200">Returns the user</response>
    /// <response code="401">Token is invalid</response>
    [HttpGet]
    public async Task<ApiResponse<GetCurrentUserResponse>> GetCurrentUser(CancellationToken cancellationToken)
        => await mediator.Send(new GetCurrentUserRequest(), cancellationToken);

    /// <summary>
    /// Generated a token to be authenticated with
    /// </summary>
    /// <response code="200">Return a brief information about authenticated user and the token to authenticate</response>
    /// <response code="400">Email or password does not match or does not exist</response>
    /// <response code="406">Email or password is invalid</response>
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request, cancellationToken);
}