using AccountHub.Domain.Entities;
using AccountHub.Domain.Repositories;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Infrastructure.Repositories;

public class RefreshTokenRepository:IRefreshTokenRepository
{
    private readonly DataContext _context;

    public RefreshTokenRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<RefreshTokenEntity?> GetRefreshToken(string userId, string deviceId)
    {
        var refreshToken =await _context.RefreshTokens.SingleOrDefaultAsync(x => x.UserId == userId && x.DeviceId == deviceId);
        return refreshToken;
    }

    public async Task<RefreshTokenEntity> AddRefreshToken(RefreshTokenEntity refreshToken)
    {
        var result = _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return result.Result.Entity;
    }

    public async Task<int> DeleteRefreshToken(string userId, string deviceId)
    {
        
       var totalDeletedTokens =await _context.RefreshTokens.Where(x=> x.UserId == userId && x.DeviceId == deviceId)
            .ExecuteDeleteAsync();
       
       return totalDeletedTokens;
        
    }
}