using AccountHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountHub.Infrastructure.Data.Configurations;

public class GameAccountConfiguration:IEntityTypeConfiguration<GameAccount>
{
    public void Configure(EntityTypeBuilder<GameAccount> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(p=>p.Characteristics).HasColumnType("jsonb");

        builder.HasMany(p => p.Images)
            .WithOne(p => p.GameAccount)
            .HasForeignKey(p => p.GameAccountId);
        builder.HasOne(p => p.Game)
            .WithMany(p => p.Accounts);

    }
}