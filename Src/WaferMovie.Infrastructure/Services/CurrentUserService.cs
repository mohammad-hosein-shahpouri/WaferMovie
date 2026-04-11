namespace WaferMovie.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContext) : ICurrentUserService
{
    public bool IsAuthenticated => httpContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public Guid Id => IsAuthenticated ?
        Guid.Parse(httpContext.HttpContext!.User.Claims.First(f => f.Type == ClaimTypes.NameIdentifier).Value) :
        throw new InvalidOperationException("Id is accessible only when authenticated");
    public string Email => IsAuthenticated ?
        httpContext.HttpContext!.User.Claims.First(f => f.Type == ClaimTypes.Email).Value :
        throw new InvalidOperationException("Email is accessible only when authenticated");
}