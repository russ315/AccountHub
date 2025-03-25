using AccountHub.Domain.Entities;
using AccountHub.Domain.Repositories;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Infrastructure.Repositories;

public class AccountImageRepository:IAccountImageRepository
{
    private readonly DataContext _context;
    private IAccountImageRepository _accountImageRepositoryImplementation;
    private IAccountImageRepository _accountImageRepositoryImplementation1;

    public AccountImageRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<AccountImageEntity?> GetImageById(int imageId,CancellationToken cancellationToken)
    {
        var image =await _context.AccountImages.FirstOrDefaultAsync(image => image.Id == imageId, cancellationToken);
        return image;
    }

    public async Task<IEnumerable<AccountImageEntity>> GetImagesByGameAccount(int gameAccountId,CancellationToken cancellationToken)
    {
        var images = _context.AccountImages.Where(image => image.GameAccountId == gameAccountId);
        return await images.ToListAsync(cancellationToken);
    }


    
    public async Task<AccountImageEntity?> AddAccountImage(AccountImageEntity accountImageEntity)
    {
        var result = await _context.AccountImages.AddAsync(accountImageEntity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<AccountImageEntity> UpdateGameAccountImage(AccountImageEntity accountImageEntity)
    {
        var result = _context.AccountImages.Update(accountImageEntity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }
}