namespace WaferMovie.Application.Groups.Queries.FindGroupById;

public record FindGroupByIdQuery(int GroupId) : IRequest<CrudResult<FindGroupByIdQueryDto>>;

public class FindGroupByIdQueryHandler : IRequestHandler<FindGroupByIdQuery, CrudResult<FindGroupByIdQueryDto>>
{
    private readonly IApplicationDbContext dbContext;

    public FindGroupByIdQueryHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<FindGroupByIdQueryDto>> Handle(FindGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Groups.ProjectToType<FindGroupByIdQueryDto>()
            .FirstOrDefaultAsync(f => f.Id == request.GroupId, cancellationToken);

        if (group is null)
            return new CrudResult<FindGroupByIdQueryDto>(CrudStatus.NotFound);

        return new CrudResult<FindGroupByIdQueryDto>(CrudStatus.Succeeded, group);
    }
}