namespace WaferMovie.Application.Common.Behaviors;

public class LocalizationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : CrudResult
{
    private readonly ILocalizationService localizationService;

    public LocalizationPipelineBehavior(ILocalizationService localizationService)
    {
        this.localizationService = localizationService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();
        if (!response.Messages.Any())
            response.Messages.Add(localizationService.FromSharedResources(response.Status.ToString()));
        else if (response.Messages.Count == 1 && response.Messages[0] == response.Status.ToString())
            response.Messages = new List<string> {
                localizationService.FromSharedResources(response.Status.ToString())
            };

        return response;
    }
}