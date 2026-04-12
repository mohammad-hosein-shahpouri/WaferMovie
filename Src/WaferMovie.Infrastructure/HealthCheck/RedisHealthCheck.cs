using Microsoft.Extensions.Configuration;

namespace WaferMovie.Infrastructure.HealthCheck;

public class RedisHealthCheck(IConfiguration configuration) : IHealthCheck
{

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("redis")!;
            using var connection = ConnectionMultiplexer.Connect(connectionString);

            var database = connection.GetDatabase();
            var result = await database.ExecuteAsync("PING");

            if (result != null && result.ToString()!.Equals("PONG", StringComparison.OrdinalIgnoreCase))
                return HealthCheckResult.Healthy("Redis is up and running.");
            else return HealthCheckResult.Unhealthy("Redis is not responding correctly.");
        }
        catch (Exception)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, "Redis is down.");
        }
    }
}