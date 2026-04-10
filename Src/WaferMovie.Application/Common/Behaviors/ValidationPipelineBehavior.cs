using Microsoft.AspNetCore.Http;

namespace WaferMovie.Application.Common.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, IHttpContextAccessor httpContext) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ApiResponse, new()
{
    private readonly HttpContext httpContext = httpContext.HttpContext!;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var errors = validators.Select(validator => validator.ValidateAsync(request).Result)
                 .SelectMany(validationResult => validationResult.Errors)
                 .Where(validationFailure => validationFailure != null)
                 .Select(s => s.ErrorMessage)
                 .Distinct()
                 .ToList();

            if (errors.Count != 0)
            {
                httpContext.Response.StatusCode = 406;
                return new TResponse
                {
                    Status = EnumApiResponseStatus.InvalidData,
                    Messages = errors,
                };
            }
        }

        return await next(cancellationToken);
    }
}