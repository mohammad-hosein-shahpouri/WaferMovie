using WaferMovie.Application.Groups.Commands.CreateGroup;
using WaferMovie.Application.Groups.Commands.DeleteGroup;
using WaferMovie.Application.Groups.Commands.UpdateGroup;
using WaferMovie.Application.Groups.Queries.FindGroupById;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
public class GroupsController : ControllerBase
{
    private readonly IMediator mediator;

    public GroupsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region Queries

    [HttpGet("{groupId}")]
    public async Task<CrudResult<FindGroupByIdQueryDto>> FindById(int groupId, CancellationToken cancellationToken)
        => await mediator.Send(new FindGroupByIdQuery(groupId), cancellationToken);

    #endregion Queries

    #region Commands

    [HttpPost]
    public async Task<CrudResult> CreateGroup(CreateGroupCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpPut]
    public async Task<CrudResult> UpdateGroup(UpdateGroupCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);

    [HttpDelete("{groupId}")]
    public async Task<CrudResult> DeleteGroup(int groupId, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteGroupCommand(groupId), cancellationToken);

    #endregion Commands
}