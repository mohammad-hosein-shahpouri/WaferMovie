using WaferMovie.Domain.Entities;

namespace WaferMovie.Application.Common.Interfaces;

public interface IJwtServices
{
    string GenerateAsync(User user);
}