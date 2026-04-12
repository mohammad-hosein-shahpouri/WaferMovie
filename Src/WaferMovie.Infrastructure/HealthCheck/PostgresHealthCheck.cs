using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WaferMovie.Infrastructure.HealthCheck;

public class PostgresHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("postgres");
            using (var connection = new NpgsqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open) await connection.OpenAsync(cancellationToken);

                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                    return HealthCheckResult.Healthy("Postgres is up and running.");
                }
            }

            return new HealthCheckResult(context.Registration.FailureStatus, "Postgres is down.");
        }
        catch (Exception)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, "Postgres is down.");
        }
    }
}