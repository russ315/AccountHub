using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Services;

public interface IJwtService
{
    Task<string> GenerateJwtAccessToken(UserEntity user);
    Task<bool> ValidateJwtToken(string jwtToken);
    string GenerateRefreshToken();
    Task<string> GetUserIdFromExpiredToken(string jwtToken);
}