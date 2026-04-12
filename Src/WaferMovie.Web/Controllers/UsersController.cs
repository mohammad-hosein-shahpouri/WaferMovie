using WaferMovie.Domain.ViewModels.Users.CreateUser;
using WaferMovie.Domain.ViewModels.Users.DeleteUser;
using WaferMovie.Domain.ViewModels.Users.UpdateUser;

namespace WaferMovie.Web.Controllers;

[Authorize]
[ApiController, ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Returns id of the newly created user</response>
    /// <response code="401">Token is invalid</response>
    /// <response code="406">Body is invalid</response>
    [HttpPost]
    public async Task<ApiResponse<Guid>> CreateUser(CreateUserRequest command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    /// <summary>
    /// Updates a user
    /// </summary>
    /// <param name="id">A v7 Guid representing user id</param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Returns id of the newly created user</response>
    /// <response code="401">Token is invalid</response>
    /// <response code="404">User not found</response>
    /// <response code="406">Body is invalid</response>
    [HttpPut("{id}")]
    public async Task<ApiResponse<Guid>> UpdateUser(Guid id, UpdateUserRequest command, CancellationToken cancellationToken)
        => await mediator.Send(command with { Id = id }, cancellationToken);

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">A v7 Guid representing user id</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Returns the user</response>
    /// <response code="404">User not found</response>
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteUser(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteUserRequest { Id = id }, cancellationToken);


}