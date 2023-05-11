using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace WaferMovie.Application.Accounts.Queries;

public record GetCurrentUserQuery : IRequest<CrudResult<GetCurrentUserQueryDto>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CrudResult<GetCurrentUserQueryDto>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IHttpContextAccessor httpContext;
    private readonly IDatabase cacheDb;

    public GetCurrentUserQueryHandler(IApplicationDbContext dbContext, IHttpContextAccessor httpContext,
        IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.httpContext = httpContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<GetCurrentUserQueryDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var username = httpContext.HttpContext?.User.Identity?.Name?.ToUpper();
        var cacheKey = $"WaferMovie:Users:{username}";
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new CrudResult<GetCurrentUserQueryDto>(CrudStatus.Succeeded, JsonConvert.DeserializeObject<GetCurrentUserQueryDto>(cacheResult!)!);

        var user = await dbContext.Users.Where(w => w.NormalizedUserName == username)
            .ProjectToType<GetCurrentUserQueryDto>().FirstOrDefaultAsync(cancellationToken);
        if (user == null)
            return new CrudResult<GetCurrentUserQueryDto>(CrudStatus.NotFound);

        var cacheValue = JsonConvert.SerializeObject(user);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new CrudResult<GetCurrentUserQueryDto>(CrudStatus.Succeeded, user);
    }
}