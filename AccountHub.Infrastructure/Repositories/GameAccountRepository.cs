using AccountHub.Domain.Entities;
using AccountHub.Domain.Repositories;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Infrastructure.Repositories;

public class GameAccountRepository:IGameAccountRepository
{
    private readonly DataContext _context;

    public GameAccountRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<GameAccountEntity?> GetAccountById(long id, CancellationToken cancellationToken)
    {
        var gameAccount = await _context.GameAccounts
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.DeletedAt == null && p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return gameAccount;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetAccountsByUsername(string username, CancellationToken cancellationToken)
    {
        var games = await _context.GameAccounts
            .Include(p => p.Seller)
            .Where(p => p.IsActive && p.DeletedAt == null && 
                      (p.Seller != null && p.Seller.UserName == username))
            .ToListAsync(cancellationToken); 
        
        return games;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetAccountsByGame(string gameName, CancellationToken cancellationToken)
    {
        var games = await _context.GameAccounts
            .Include(p => p.Game)
            .Where(p => p.IsActive && p.DeletedAt == null &&
                      (p.Game != null && p.Game.Name == gameName))
            .ToListAsync(cancellationToken);
        return games;
    }

    public async Task<GameAccountEntity> AddGameAccount(GameAccountEntity gameEntity)
    {
        var result =await _context.GameAccounts.AddAsync(gameEntity);
        await _context.SaveChangesAsync();
        
        return result.Entity;
    }

    public async Task<GameAccountEntity> DeleteGameAccount(GameAccountEntity entity)
    {
        entity.DeletedAt = DateTime.UtcNow;
        entity.IsActive = false;
        
        _context.GameAccounts.Update(entity);
        await _context.SaveChangesAsync();
        
        return entity; 
    }

    public async Task<GameAccountEntity?> UpdateGameAccount(GameAccountEntity entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateConcurrencyException)
        {
            // The entity has been modified by another user
            return null;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}