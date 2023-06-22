using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using System.Data;

namespace WaferMovie.Infrastructure.HealthCheck;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IConfiguration configuration;

    public DatabaseHealthCheck(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

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
                    return HealthCheckResult.Healthy("The database is up and running.");
                }
            }

            return new HealthCheckResult(context.Registration.FailureStatus, "The database is down.");
        }
        catch (Exception)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, "The database is down.");
        }
    }
}