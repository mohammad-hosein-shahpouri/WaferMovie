namespace WaferMovie.Domain.Interfaces;

public interface ITokenServices
{
    string GenerateJwtAsync(User user);
}