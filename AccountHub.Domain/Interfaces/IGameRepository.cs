using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Interfaces;

public interface IGameRepository
{
    Task<Game?> GetById(long id,CancellationToken cancellationToken=default);
}