using AccountHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountHub.Infrastructure.Data.Configurations;

public class GameVariantConfiguration:IEntityTypeConfiguration<GameVariantEntity>
{
    public void Configure(EntityTypeBuilder<GameVariantEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ValidationRules).HasColumnType("jsonb");

        builder.HasOne(p => p.Game)
            .WithMany(p => p.Variants)
            .HasForeignKey(p => p.GameId);
    }
}