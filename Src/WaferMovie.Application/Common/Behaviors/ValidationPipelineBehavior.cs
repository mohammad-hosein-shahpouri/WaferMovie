namespace WaferMovie.Application.Common.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : CrudResult, new()
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

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

            if (errors.Any()) return new TResponse
            {
                Status = CrudStatus.ValidationError,
                Messages = errors
            };
        }

        return await next();
    }
}