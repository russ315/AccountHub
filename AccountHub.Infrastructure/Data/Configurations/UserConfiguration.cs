using AccountHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountHub.Infrastructure.Data.Configurations;

public class UserConfiguration:IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(p=>p.Id);
        //builder.Property(p => p.Balance);
        builder.Property(p=>p.ImageUrl).HasMaxLength(256);

        builder.HasMany(p => p.Notifications)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);
        builder.HasMany(p=>p.Transactions)
            .WithOne(p=>p.Buyer)
            .HasForeignKey(p=>p.BuyerId);
        builder.HasMany(p=>p.Notifications)
            .WithOne(p=>p.User)
            .HasForeignKey(p=>p.UserId);
        builder.HasMany(p=>p.GameAccounts)
            .WithOne(p=>p.Seller)
            .HasForeignKey(p=>p.SellerId);
        builder.HasMany(p=>p.ServicesProvided)
            .WithOne(p=>p.Provider)
            .HasForeignKey(p=>p.ProviderId);
        
        builder.HasMany(p=>p.ReviewsGiven)
            .WithOne(p=>p.Reviewer)
            .HasForeignKey(p=>p.ReviewerId);
        builder.HasMany(p=>p.ReviewsReceived)
            .WithOne(p=>p.Reviewee)
            .HasForeignKey(p=>p.RevieweeId);
        

    }
}