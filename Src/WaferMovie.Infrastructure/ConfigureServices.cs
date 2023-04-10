using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Domain.Entities;
using WaferMovie.Infrastructure.Persistence;
using WaferMovie.Infrastructure.Services;

namespace WaferMovie.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies()
            .UseNpgsql(configuration.GetConnectionString("postgres")));

        services.AddRedis(configuration);

        services.AddDependencyInjection();

        services.AddIdentity<User, Domain.Entities.Role>()
            .AddDefaultTokenProviders()
         .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    private static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IJwtServices, JwtServices>();
        services.AddScoped<IEmailServices, EmailServices>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisPort = configuration["Redis:Port"];
        var redisHost = configuration["Redis:Host"];
        var redisPassword = configuration["Redis:Password"];
        var redisDatabase = configuration.GetValue<int>("Redis:Database");
        var connectionString = $"{redisHost}:{redisPort},password={redisPassword}";

        services.AddScoped(cfg => ConnectionMultiplexer.Connect(connectionString).GetDatabase(redisDatabase));

        return services;
    }
}