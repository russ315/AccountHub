using AccountHub.Domain.Entities;
using AccountHub.Domain.Repositories;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Infrastructure.Repositories;

public class GameRepository: IGameRepository
{
    private readonly DataContext _context;

    public GameRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<GameEntity?> GetById(long id, CancellationToken cancellationToken=default)
    {
        var game =await _context.Games.FirstOrDefaultAsync(p=>p.Id==id, cancellationToken);
        return game;
    }

    public  async Task<GameEntity?> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var game =await _context.Games.FirstOrDefaultAsync(p=>p.Name==name, cancellationToken);
        return game;    
    }

    public async Task<GameEntity?> AddGame(GameEntity gameEntity, CancellationToken cancellationToken = default)
    {
        
        var gameResult = await _context.Games.AddAsync(gameEntity,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return gameResult.Entity;
    }

    public async Task<int> DeleteGame(long gameId, CancellationToken cancellationToken = default)
    {
        var result = await _context.Games.Where(p=>p.Id==gameId).ExecuteDeleteAsync(cancellationToken);
        return result;
    }
}