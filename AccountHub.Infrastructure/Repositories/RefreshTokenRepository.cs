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
    public async Task<RefreshTokenEntity?> GetRefreshToken(string userId, string deviceId, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .Where(x => x.IsActive && x.DeletedAt == null && 
                     x.UserId == userId && x.DeviceId == deviceId)
            .SingleOrDefaultAsync(cancellationToken);
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
        var entity = await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.UserId == userId && x.DeviceId == deviceId && x.DeletedAt == null);
        
        if (entity == null)
            return 0;
        
        // Implement soft delete
        entity.DeletedAt = DateTime.UtcNow;
        entity.IsActive = false;
        
        // Update the entity
        _context.RefreshTokens.Update(entity);
        await _context.SaveChangesAsync();
        
        return 1; // One record affected
    }
}