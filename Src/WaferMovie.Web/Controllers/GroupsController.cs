using WaferMovie.Domain.ViewModels.Groups.CreateGroup;
using WaferMovie.Domain.ViewModels.Groups.DeleteGroup;
using WaferMovie.Domain.ViewModels.Groups.GetGroupById;
using WaferMovie.Domain.ViewModels.Groups.UpdateGroup;

namespace WaferMovie.Web.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController, ApiVersion("1.0")]
public class GroupsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ApiResponse<GetGroupByIdResponse>> GetGroupById(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new GetGroupByIdRequest { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<Guid>> CreateGroup(CreateGroupRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ApiResponse> UpdateGroup(Guid id, UpdateGroupRequest request, CancellationToken cancellationToken)
        => await mediator.Send(request with { Id = id }, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteGroup(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteGroupRequest { Id = id }, cancellationToken);
}