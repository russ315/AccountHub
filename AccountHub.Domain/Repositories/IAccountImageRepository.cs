using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IAccountImageRepository
{
    Task<AccountImageEntity?> GetImageById(int imageId,CancellationToken cancellationToken);
    Task<IEnumerable<AccountImageEntity>> GetImagesByGameAccount(int gameAccountId,CancellationToken cancellationToken);
    Task<AccountImageEntity?> AddAccountImage(AccountImageEntity accountImageEntity);
    Task<AccountImageEntity> UpdateGameAccountImage(AccountImageEntity accountImageEntity);
    


}