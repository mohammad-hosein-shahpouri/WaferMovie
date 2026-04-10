using WaferMovie.Domain.ViewModels.Groups.CreateGroup;
using WaferMovie.Domain.ViewModels.Groups.DeleteGroup;
using WaferMovie.Domain.ViewModels.Groups.GetGroupById;
using WaferMovie.Domain.ViewModels.Groups.UpdateGroup;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController, ApiVersion("1.0")]
public class GroupsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ApiResponse<GetGroupByIdResponse>> FindById(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new GetGroupByIdRequest { Id = id }, cancellationToken);

    [HttpPost]
    public async Task<ApiResponse<Guid>> CreateGroup(CreateGroupRequest command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ApiResponse> UpdateGroup(Guid id, UpdateGroupRequest command, CancellationToken cancellationToken)
        => await mediator.Send(command with { Id = id }, cancellationToken);

    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteGroup(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteGroupRequest { Id = id }, cancellationToken);
}