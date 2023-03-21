using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Infrastructure.Services;

namespace WaferMovie.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies()
            .UseNpgsql(configuration.GetConnectionString("postgres")));

        services.AddDependencyInjection();

        return services;
    }

    private static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IJwtServices, JwtServices>();
        services.AddScoped<IEmailServices, EmailServices>();

        return services;
    }
}