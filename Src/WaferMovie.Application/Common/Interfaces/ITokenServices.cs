using WaferMovie.Domain.Entities;

namespace WaferMovie.Application.Common.Interfaces;

public interface ITokenServices
{
    string GenerateJwtAsync(User user);
}