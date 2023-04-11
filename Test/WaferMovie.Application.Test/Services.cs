using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WaferMovie.Domain.Entities;

namespace WaferMovie.Application.Test;

public static class Services
{
    public static IMediator Configure(IApplicationDbContext dbContext)
    {
        ServiceCollection services = new ServiceCollection();
        var applicationAssembly = Assembly.Load("WaferMovie.Application");
        var mockLocalization = new Mock<ILocalizationService>();
        mockLocalization.Setup(a => a.FromPropertyResources(It.IsAny<string>())).Returns<string>(r => r);
        mockLocalization.Setup(a => a.FromSharedResources(It.IsAny<string>())).Returns<string>(r => r);
        mockLocalization.Setup(a => a.FromValidationResources(It.IsAny<string>())).Returns<string>(r => r);
        var mockCacheDb = new Mock<IDatabase>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton(dbContext);
        services.AddSingleton(mockLocalization.Object);
        services.AddSingleton(mockCacheDb.Object);

        return services.BuildServiceProvider().GetService<IMediator>()!;
    }
}