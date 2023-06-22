using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WaferMovie.Application.Common.Behaviors;

namespace WaferMovie.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddPipelines();
        services.AddMapster();
        return services;
    }

    private static IServiceCollection AddPipelines(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LocalizationPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(HttpStatusCodePipelineBehavior<,>));

        return services;
    }

    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        IList<IRegister> registers = config.Scan(Assembly.GetExecutingAssembly());
        config.Apply(registers);
        services.AddSingleton(config);

        return services;
    }
}