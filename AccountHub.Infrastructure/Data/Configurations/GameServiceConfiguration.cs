﻿using AccountHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountHub.Infrastructure.Data.Configurations;

public class GameServiceConfiguration:IEntityTypeConfiguration<GameService>
{
    public void Configure(EntityTypeBuilder<GameService> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceMetadata).HasColumnType("jsonb");

        builder.HasMany(p => p.Schedules)
            .WithOne(p => p.GameService)
            .HasForeignKey(p => p.GameServiceId);
    }
}