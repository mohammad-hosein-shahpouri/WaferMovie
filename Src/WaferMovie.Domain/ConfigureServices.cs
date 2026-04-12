using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaferMovie.Domain.Options;

namespace WaferMovie.Domain;

public static class ConfigureServices
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionMappings(configuration);
        return services;
    }

    public static IServiceCollection AddOptionMappings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(options => configuration.GetSection(JwtOptions.CONFIG));
        services.Configure<EmailOptions>(options => configuration.GetSection(EmailOptions.CONFIG));

        return services;
    }
}