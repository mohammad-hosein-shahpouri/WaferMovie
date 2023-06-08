using Microsoft.AspNetCore.Http;
using WaferMovie.Application.Common.Interfaces;

namespace WaferMovie.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContext;

    public CurrentUserService(IHttpContextAccessor httpContext)
    {
        this.httpContext = httpContext;
    }

    public bool IsAuthenticated => httpContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public int? Id => IsAuthenticated ? Convert.ToInt32(httpContext.HttpContext?.User.Claims.FirstOrDefault(f => f.Type == "UserId")?.Value) : null;
}