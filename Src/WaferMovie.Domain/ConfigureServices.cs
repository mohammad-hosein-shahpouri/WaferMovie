using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaferMovie.Domain.Options;

namespace WaferMovie.Domain;

public static class ConfigureServices
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisOptions>(options => configuration.GetSection(RedisOptions.CONFIG));

        return services;
    }
}