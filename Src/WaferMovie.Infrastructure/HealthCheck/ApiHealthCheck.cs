using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WaferMovie.Infrastructure.HealthCheck;

public class ApiHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory httpClientFactory;

    public ApiHealthCheck(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}