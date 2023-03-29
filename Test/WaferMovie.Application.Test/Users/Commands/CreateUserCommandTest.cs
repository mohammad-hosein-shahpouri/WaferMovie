using WaferMovie.Application.Users.Commands.CreateUser;

namespace WaferMovie.Application.Test.Users.Commands;

public class CreateUserCommandTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public CreateUserCommandTest()
    {
        var applicationAssembly = Assembly.Load("WaferMovie.Application");
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddSingleton(dbContext);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        mediator = services.BuildServiceProvider().GetService<IMediator>()!;
    }

    public async Task ShouldCreateUser()
    {
        var command = new CreateUserCommand
        {
        };
    }
}