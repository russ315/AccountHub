using AccountHub.Domain.Entities;
using AccountHub.Domain.Repositories;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Infrastructure.Repositories;

public class GameServiceRepository:IGameServiceRepository
{
    private readonly DataContext _context;

    public GameServiceRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<GameServiceEntity?> GetAccountServiceById(long id, CancellationToken cancellationToken)
    {
        var gameService = await _context.GameServices
            .Where(p => p.IsActive && p.DeletedAt == null && p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return gameService;
    }

    public async Task<IEnumerable<GameServiceEntity>> GetAccountServicesByUsername(string username, CancellationToken cancellationToken)
    {
        var gameServices = await _context.GameServices
            .Include(p => p.Provider)
            .Where(p => p.IsActive && p.DeletedAt == null && 
                     (p.Provider != null && p.Provider.UserName == username))
            .ToListAsync(cancellationToken);
        
        return gameServices;
    }

    public async Task<IEnumerable<GameServiceEntity>> GetAccountServicesByGame(string gameName,
        CancellationToken cancellationToken)
    {
        var gameServices = await _context.GameServices
            .Include(p => p.Game)
            .Where(p => p.IsActive && p.DeletedAt == null && 
                     (p.Game != null && p.Game.Name == gameName))
            .ToListAsync(cancellationToken);
        return gameServices;
    }

    public async Task<GameServiceEntity> AddGameService(GameServiceEntity entity)
    {
        var result =await _context.GameServices.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
        
    }

    public async Task<int> DeleteGameService(long id)
    {
        var entity = await _context.GameServices.FindAsync(id);
        if (entity == null)
            return 0;
        
        // Implement soft delete
        entity.DeletedAt = DateTime.UtcNow;
        entity.IsActive = false;
        
        // Update the entity
        _context.GameServices.Update(entity);
        await _context.SaveChangesAsync();
        
        return 1; // One record affected
    }
}