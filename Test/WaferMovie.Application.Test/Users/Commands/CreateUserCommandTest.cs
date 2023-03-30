using Microsoft.AspNetCore.Identity;
using WaferMovie.Application.Users.Commands.CreateUser;
using WaferMovie.Domain.Entities;

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
        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        mediator = services.BuildServiceProvider().GetService<IMediator>()!;
    }

    [Fact]
    public async Task ShouldCreateUser()
    {
        var command = new CreateUserCommand
        {
            Email = "mail@example.com",
            Name = "Test",
            Password = "!Q2wE4rg",
            PasswordConfirmation = "!Q2wE4rg",
            PhoneNumber = "+989135949659",
            UserName = "Test",
        };

        var result = await mediator.Send(command);

        if (result.Succeeded)
        {
            result.Data.ShouldNotBeNull();
            result.Data.PasswordHash.ShouldNotBeNull();
            result.Data.SecurityStamp.ShouldNotBeNull();
            result.Data.NormalizedEmail.ShouldNotBeNull()
                .ShouldBe(command.Email.ToUpper());
            result.Data.NormalizedUserName.ShouldNotBeNull()
                .ShouldBe(command.UserName.ToUpper());
        }
        else
            Assert.Fail(string.Join("-", result.Messages));
    }
}