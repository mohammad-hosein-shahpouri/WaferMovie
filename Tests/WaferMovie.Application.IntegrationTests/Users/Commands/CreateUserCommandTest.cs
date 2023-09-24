using WaferMovie.Application.Users.Commands.CreateUser;

namespace WaferMovie.Application.IntegrationTests.Users.Commands;

public class CreateUserCommandTest : TestFixture
{
    [Test]
    public async Task ShouldCreateUser()
    {
        var mediator = Resolve<IMediator>();

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
            result.Data.Should().NotBeNull();
            result.Data.PasswordHash.Should().NotBeNull();
            result.Data.SecurityStamp.Should().NotBeNull();
            result.Data.NormalizedEmail.Should().NotBeNull();
            result.Data.NormalizedEmail.Should().Be(command.Email.ToUpper());
            result.Data.NormalizedUserName.Should().NotBeNull();
            result.Data.NormalizedUserName.Should().Be(command.UserName.ToUpper());
        }
        else
            Assert.Fail(string.Join("-", result.Messages));
    }
}