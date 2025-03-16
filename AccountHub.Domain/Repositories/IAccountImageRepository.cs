using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IAccountImageRepository
{
    Task<AccountImageEntity?> GetImageById(int imageId);
    Task<IEnumerable<AccountImageEntity>> GetImagesByGameAccount(int gameAccountId);
    Task<AccountImageEntity?> AddAccountImage(AccountImageEntity accountImageEntity);
    Task<AccountImageEntity> UpdateGameAccountImage(AccountImageEntity accountImageEntity);
    


}