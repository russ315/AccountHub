﻿// <auto-generated />
using System;
using System.Text.Json;
using AccountHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccountHub.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250316152850_added text field for ReviewEntity and balance for service")]
    partial class addedtextfieldforReviewEntityandbalanceforservice
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AccountHub.Domain.Entities.AccountImageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("GameAccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameAccountId");

                    b.ToTable("AccountImages");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameAccountEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<JsonDocument>("Characteristics")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CurrentOwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("SellerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CurrentOwnerId");

                    b.HasIndex("GameId");

                    b.HasIndex("SellerId");

                    b.ToTable("GameAccounts");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<JsonDocument>("Metadata")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("lower(Name) = lower(Name)");

                    NpgsqlIndexBuilderExtensions.HasOperators(b.HasIndex("Name"), new[] { "text_pattern_ops" });

                    b.ToTable("Games");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameServiceEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("ProviderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<JsonDocument>("ServiceMetadata")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("ProviderId");

                    b.ToTable("GameServices");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameVariantEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<JsonDocument>("ValidationRules")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("GameVariants");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.NotificationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.ReviewEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Review")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RevieweeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ReviewerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Stars")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RevieweeId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.ServiceScheduleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("ClientId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("GameServiceId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsBooked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("GameServiceId");

                    b.ToTable("ServiceSchedules");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.TransactionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("GameAccountId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("GameAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.AccountImageEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.GameAccountEntity", "GameAccount")
                        .WithMany("Images")
                        .HasForeignKey("GameAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameAccount");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameAccountEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "CurrentOwner")
                        .WithMany()
                        .HasForeignKey("CurrentOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountHub.Domain.Entities.GameEntity", "Game")
                        .WithMany("Accounts")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "Seller")
                        .WithMany("GameAccounts")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentOwner");

                    b.Navigation("Game");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameServiceEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.GameEntity", "Game")
                        .WithMany("Services")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "Provider")
                        .WithMany("ServicesProvided")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameVariantEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.GameEntity", "Game")
                        .WithMany("Variants")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.NotificationEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.RefreshTokenEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.ReviewEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "Reviewee")
                        .WithMany("ReviewsReceived")
                        .HasForeignKey("RevieweeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "Reviewer")
                        .WithMany("ReviewsGiven")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reviewee");

                    b.Navigation("Reviewer");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.ServiceScheduleEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("AccountHub.Domain.Entities.GameServiceEntity", "GameService")
                        .WithMany("Schedules")
                        .HasForeignKey("GameServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("GameService");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.TransactionEntity", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", "Buyer")
                        .WithMany("Transactions")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountHub.Domain.Entities.GameAccountEntity", "GameAccount")
                        .WithMany()
                        .HasForeignKey("GameAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("GameAccount");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountHub.Domain.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AccountHub.Domain.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameAccountEntity", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameEntity", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Services");

                    b.Navigation("Variants");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.GameServiceEntity", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("AccountHub.Domain.Entities.UserEntity", b =>
                {
                    b.Navigation("GameAccounts");

                    b.Navigation("Notifications");

                    b.Navigation("ReviewsGiven");

                    b.Navigation("ReviewsReceived");

                    b.Navigation("ServicesProvided");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
