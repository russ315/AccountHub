using AccountHub.Domain.Entities;
using AccountHub.Domain.Repositories;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Infrastructure.Repositories;

public class AccountImageRepository:IAccountImageRepository
{
    private readonly DataContext _context;

    public AccountImageRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<AccountImageEntity?> GetImageById(long imageId,CancellationToken cancellationToken)
    {
        var image =await _context.AccountImages.FirstOrDefaultAsync(image => image.Id == imageId, cancellationToken);
        return image;
    }

    public async Task<IEnumerable<AccountImageEntity>> GetImagesByGameAccount(long gameAccountId,CancellationToken cancellationToken)
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

    public async Task DeleteAccountImage(long imageId)
    {
        var image = await _context.AccountImages.FindAsync(imageId);
        if (image != null)
        {
            _context.AccountImages.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}