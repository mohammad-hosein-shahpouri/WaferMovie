using StackExchange.Redis;

namespace WaferMovie.Infrastructure.HealthCheck;

public class RedisHealthCheck(IOptions<RedisOptions> options) : IHealthCheck
{
    private readonly RedisOptions redisOptions = options.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = $"{redisOptions.Host}:{redisOptions.Port},password={redisOptions.Password}";
            //var connection = ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
            using var connection = ConnectionMultiplexer.Connect(connectionString);

            var database = connection.GetDatabase(redisOptions.Database);
            var result = await database.ExecuteAsync("PING");

            if (result != null && result.ToString()!.Equals("PONG", StringComparison.OrdinalIgnoreCase)) return HealthCheckResult.Healthy("Redis is up and running.");
            else return HealthCheckResult.Unhealthy("Redis is not responding correctly.");
        }
        catch (Exception)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, "Redis is down.");
        }
    }
}