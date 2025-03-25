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
        var game =await _context.Games.FirstOrDefaultAsync(p=>p.Id==id, cancellationToken);
        return game;
    }

    public  async Task<GameEntity?> GetByName(string name, CancellationToken cancellationToken)
    {
        var game =await _context.Games.FirstOrDefaultAsync(p=>p.Name==name, cancellationToken);
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
        var result = await _context.Games.Where(p=>p.Id==gameId).ExecuteDeleteAsync();
        return result;
    }
}