using Microsoft.AspNetCore.Http;
using WaferMovie.Domain.Interfaces;

namespace WaferMovie.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContext;

    public CurrentUserService(IHttpContextAccessor httpContext)
    {
        this.httpContext = httpContext;
    }

    public bool IsAuthenticated => httpContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public Guid Id => IsAuthenticated ? Guid.Parse(httpContext.HttpContext!.User.Claims.First(f => f.Type == "UserId").Value) : Guid.Empty;
}