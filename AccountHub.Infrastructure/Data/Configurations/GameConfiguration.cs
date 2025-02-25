using AccountHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountHub.Infrastructure.Data.Configurations;

public class GameConfiguration:IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(p=>p.Name).IsUnique()
            .HasFilter("lower(Name) = lower(Name)")
            .HasOperators("text_pattern_ops"); 
        builder.Property(x => x.Metadata).HasColumnType("jsonb");
        
        builder.HasMany(p=>p.Services)
            .WithOne(p=>p.Game)
            .HasForeignKey(p=>p.GameId);
        builder.HasMany(p=>p.Accounts)
            .WithOne(p=>p.Game)
            .HasForeignKey(p=>p.GameId);
        builder.HasMany(p=>p.Variants)
            .WithOne(p=>p.Game)
            .HasForeignKey(p=>p.GameId);
        
    }
}