using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Services;

public interface IJwtService
{
    string GenerateJwtAccessToken(UserEntity user);
    Task<bool> ValidateJwtToken(string jwtToken);
    string GenerateRefreshToken();
    Task<string> GetUsernameFromExpiredToken(string jwtToken);
}