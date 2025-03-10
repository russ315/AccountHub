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
    public async Task<GameAccountEntity?> GetAccountById(long id, CancellationToken cancellationToken=default)
    {
        var gameAccount = await _context.GameAccounts.FirstOrDefaultAsync(p=>p.Id==id, cancellationToken);
        return gameAccount;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetAccountsByUsername(string username, CancellationToken cancellationToken=default)
    {
        var games = await _context.GameAccounts
            .Include(p=>p.Seller)
            .Where(p=>(p.Seller!=null && p.Seller.UserName==username)
                      ).ToListAsync(cancellationToken); 
        
        return games;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetAccountsByGame(string gameName, CancellationToken cancellationToken=default)
    {
        var games = await _context.GameAccounts
            .Include(p=>p.Game)
            .Where(p=>p.Game!=null && p.Game.Name==gameName)
            .ToListAsync(cancellationToken);
        return games;
    }

    public async Task<GameAccountEntity> AddGameAccount(GameAccountEntity gameEntity, CancellationToken cancellationToken = default)
    {
        var result =await _context.GameAccounts.AddAsync(gameEntity,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<int> DeleteGameAccount(long id, CancellationToken cancellationToken = default)
    {
        var result = await _context.GameAccounts.Where(p=>p.Id==id).ExecuteDeleteAsync(cancellationToken);
        return result;
    }
}