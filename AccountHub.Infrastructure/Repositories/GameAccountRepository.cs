using AccountHub.Domain.Entities;
using AccountHub.Domain.Interfaces;
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
    public Task<GameAccount?> GetAccountById(long id, CancellationToken cancellationToken=default)
    {
        var gameAccount = _context.GameAccounts.FirstOrDefaultAsync(p=>p.Id==id, cancellationToken);
        return gameAccount;
    }

    public async Task<IEnumerable<GameAccount>> GetAccountsByUsername(string username, CancellationToken cancellationToken=default)
    {
        var games = await _context.GameAccounts
            .Include(p=>p.Seller)
            .Include(p=>p.CurrentOwner)
            .Where(p=>(p.Seller!=null && p.Seller.UserName==username)
                      ||(p.CurrentOwner!=null && p.CurrentOwner.UserName==username)).ToListAsync(cancellationToken); 
        
        return games;
    }

    public async Task<IEnumerable<GameAccount>> GetAccountsByGame(string gameName, CancellationToken cancellationToken=default)
    {
        var games = await _context.GameAccounts
            .Include(p=>p.Game)
            .Where(p=>p.Game!=null && p.Game.Name==gameName)
            .ToListAsync(cancellationToken);
        return games;
    }
}