using WaferMovie.Application.Common;

namespace WaferMovie.Application.Accounts.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<CrudResult<GetCurrentUserQueryDto>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CrudResult<GetCurrentUserQueryDto>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly ICurrentUserService currentUserService;
    private readonly IDatabase cacheDb;

    public GetCurrentUserQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService,
        IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.currentUserService = currentUserService;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<GetCurrentUserQueryDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetUsersKey(currentUserService.Id);
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<GetCurrentUserQueryDto>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<GetCurrentUserQueryDto>(cacheResult!)!);

        var user = await dbContext.Users.Where(w => w.Id == currentUserService.Id)
            .ProjectToType<GetCurrentUserQueryDto>().FirstOrDefaultAsync(cancellationToken);
        if (user == null)
            return new CrudResult<GetCurrentUserQueryDto>(CrudStatus.NotFound);

        var cacheValue = JsonConvert.SerializeObject(user);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<GetCurrentUserQueryDto>(CrudStatus.Succeeded, user);
    }
}