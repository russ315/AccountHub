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
        var gameService = await _context.GameServices.FirstOrDefaultAsync(p=>p.Id==id, cancellationToken);
        return gameService;
    }

    public async Task<IEnumerable<GameServiceEntity>> GetAccountServicesByUsername(string username, CancellationToken cancellationToken)
    {
        var gameServices =await _context.GameServices
            .Include(p => p.Provider)
            .Where(p => (p.Provider != null && p.Provider.UserName == username)).ToListAsync(cancellationToken);
        
        return gameServices;
    }

    public async Task<IEnumerable<GameServiceEntity>> GetAccountServicesByGame(string gameName,
        CancellationToken cancellationToken)
    {
        var gameServices = await _context.GameServices
            .Include(p => p.Game)
            .Where(p => p.Game != null && p.Game.Name == gameName)
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
        var result = await _context.GameAccounts.Where(p=>p.Id==id).ExecuteDeleteAsync();
        return result;
    }
}