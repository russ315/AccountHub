using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IAccountImageRepository
{
    Task<AccountImageEntity?> GetImageById(long imageId,CancellationToken cancellationToken);
    Task<IEnumerable<AccountImageEntity>> GetImagesByGameAccount(long gameAccountId,CancellationToken cancellationToken);
    Task<AccountImageEntity?> AddAccountImage(AccountImageEntity accountImageEntity);
    Task<AccountImageEntity> UpdateGameAccountImage(AccountImageEntity accountImageEntity);
    Task DeleteAccountImage(long imageId);
}