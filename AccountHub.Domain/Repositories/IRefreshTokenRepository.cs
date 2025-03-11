using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshTokenEntity?> GetRefreshToken(string userId,string deviceId);
    Task<RefreshTokenEntity> AddRefreshToken(RefreshTokenEntity refreshToken);
    Task<int> DeleteRefreshToken(string userId, string deviceId);
    

}