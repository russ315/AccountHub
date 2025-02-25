using AccountHub.Domain.Entities;
using AccountHub.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Infrastructure.Data;

public sealed class DataContext:IdentityDbContext<UserEntity>
{
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
        
    }

    public DbSet<GameEntity> Games { get; set; }
    public DbSet<GameServiceEntity> GameServices { get; set; }
    public DbSet<GameAccountEntity> GameAccounts { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<AccountImageEntity> AccountImages { get; set; }
    public DbSet<GameVariantEntity> GameVariants { get; set; }
    public DbSet<NotificationEntity> Notifications { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<ServiceScheduleEntity> ServiceSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(Activator.CreateInstance<GameAccountConfiguration>());
        builder.ApplyConfiguration(Activator.CreateInstance<GameConfiguration>());
        builder.ApplyConfiguration(Activator.CreateInstance<GameVariantConfiguration>());
        builder.ApplyConfiguration(Activator.CreateInstance<ReviewConfiguration>());
        builder.ApplyConfiguration(Activator.CreateInstance<UserConfiguration>());
        builder.ApplyConfiguration(Activator.CreateInstance<GameServiceConfiguration>());

        base.OnModelCreating(builder);
    }
}