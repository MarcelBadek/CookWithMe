using CookWithMe.Data.Entities;

namespace CookWithMe.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}