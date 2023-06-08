using WaferMovie.Application.Groups.Commands.CreateGroup;

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

    [HttpPost]
    public async Task<CrudResult> CreateGroup(CreateGroupCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);
}