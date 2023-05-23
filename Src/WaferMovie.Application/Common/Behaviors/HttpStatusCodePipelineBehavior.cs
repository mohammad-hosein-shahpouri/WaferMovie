using Microsoft.AspNetCore.Http;
using System.Net;

namespace WaferMovie.Application.Common.Behaviors;

public class HttpStatusCodePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : CrudResult
{
    private readonly HttpContext httpContext;

    public HttpStatusCodePipelineBehavior(IHttpContextAccessor httpContext)
    {
        this.httpContext = httpContext.HttpContext!;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        switch (response.Status)
        {
            case CrudStatus.NotFound:
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                break;

            case CrudStatus.ValidationError or CrudStatus.Failed:
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                break;

            case CrudStatus.Succeeded:
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                break;

            default:
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                break;
        }

        return response;
    }
}