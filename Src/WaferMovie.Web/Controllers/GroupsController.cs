using WaferMovie.Application.Groups.Commands.CreateGroup;
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

    #endregion Commands
}