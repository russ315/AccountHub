using AccountHub.Domain.Entities;
using AccountHub.Domain.Interfaces;
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
    public async Task<Game?> GetById(long id, CancellationToken cancellationToken=default)
    {
        var game =await _context.Games.FirstOrDefaultAsync(p=>p.Id==id, cancellationToken);
        return game;
    }

    

    
}