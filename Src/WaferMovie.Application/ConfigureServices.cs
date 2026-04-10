using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WaferMovie.Application.Common.Behaviors;

namespace WaferMovie.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var domainAssembly = AppDomain.CurrentDomain.Load("WaferMovie.Domain");
        var applicationAssembly = AppDomain.CurrentDomain.Load("WaferMovie.Application");


        services.AddMediatR(cfg =>
        {
            cfg.RegisterGenericHandlers = true;
            cfg.RegisterServicesFromAssemblies(applicationAssembly);
            cfg.AddOpenBehaviors([typeof(ValidationPipelineBehavior<,>),
                typeof(LocalizationPipelineBehavior<,>),
                typeof(HttpStatusCodePipelineBehavior<,>)
                ]);
        });
        services.AddValidatorsFromAssemblies([domainAssembly]);
        services.AddMapster();
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