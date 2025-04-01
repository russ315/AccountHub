using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Repositories;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AccountHub.Infrastructure.Repositories;

public class GameRepository: IGameRepository
{
    private readonly DataContext _context;

    public GameRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<GameEntity?> GetById(long id, CancellationToken cancellationToken)
    {
        var game = await _context.Games
            .Where(p => p.IsActive && p.DeletedAt == null && p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return game;
    }

    public  async Task<GameEntity?> GetByName(string name, CancellationToken cancellationToken)
    {
        var game = await _context.Games
            .Where(p => p.IsActive && p.DeletedAt == null && p.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
        return game;    
    }

    public async Task<GameEntity> AddGame(GameEntity gameEntity)
    {
        var gameResult = await _context.Games.AddAsync(gameEntity);
        try
        {
            await _context.SaveChangesAsync(); 
            return gameResult.Entity;
        }
        catch (Exception e) when (e.InnerException is PostgresException pgException &&
                                  pgException.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new DuplicateEntityException("Duplicate Game", $"Game with name {gameEntity.Name} already exists");
        }
    }

    public async Task<int> DeleteGame(long gameId)
    {
        var entity = await _context.Games.FindAsync(gameId);
        if (entity == null)
            return 0;
        
        // Implement soft delete
        entity.DeletedAt = DateTime.UtcNow;
        entity.IsActive = false;
        
        // Update the entity
        _context.Games.Update(entity);
        await _context.SaveChangesAsync();
        
        return 1; // One record affected
    }
}