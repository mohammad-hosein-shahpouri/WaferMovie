using Microsoft.AspNetCore.Http;
using WaferMovie.Domain.Common;

namespace WaferMovie.Application.Common.Behaviors;

public class HttpStatusCodePipelineBehavior<TRequest, TResponse>(IHttpContextAccessor httpContext) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ApiResponse
{
    private readonly HttpContext httpContext = httpContext.HttpContext!;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        httpContext.Response.StatusCode = Convert.ToInt32(response.Status);

        return response;
    }
}