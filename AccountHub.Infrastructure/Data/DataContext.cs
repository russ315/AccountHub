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

    public DbSet<Game> Games { get; set; }
    public DbSet<GameService> GameServices { get; set; }
    public DbSet<GameAccount> GameAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<AccountImage> AccountImages { get; set; }
    public DbSet<GameVariant> GameVariants { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ServiceSchedule> ServiceSchedules { get; set; }

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