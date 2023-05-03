using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WaferMovie.Web.Controllers;

[Route("[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    private readonly HealthCheckService healthCheckService;

    public HealthCheckController(HealthCheckService healthCheckService)
    {
        this.healthCheckService = healthCheckService;
    }

    [HttpGet]
    public async Task<HealthReport> GetAsync()
        => await healthCheckService.CheckHealthAsync();
}