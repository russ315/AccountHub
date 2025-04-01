using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AccountHub.Domain.Events;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Domain.Entities;

public class UserEntity : IdentityUser
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
    [Url]
    [MaxLength(2048)]
    public string? ImageUrl { get; private set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; private set; }

    [JsonIgnore]
    public ICollection<GameAccountEntity>? GameAccounts { get; set; }

    [JsonIgnore]
    public ICollection<TransactionEntity>? Transactions { get; set; }

    [JsonIgnore]
    public ICollection<ReviewEntity>? ReviewsGiven { get; set; }

    [JsonIgnore]
    public ICollection<ReviewEntity>? ReviewsReceived { get; set; }

    [NotMapped]
    [JsonIgnore]
    public ICollection<GameServiceEntity>? ServicesProvided { get; set; }

    [NotMapped]
    [JsonIgnore]
    public ICollection<NotificationEntity>? Notifications { get; set; }

    [NotMapped]
    [JsonIgnore]
    public ICollection<RefreshTokenEntity>? RefreshTokens { get; set; }

    [NotMapped]
    [JsonIgnore]
    public string? CurrentRole { get; set; }

    public void UpdateBalance(decimal newBalance, string reason)
    {
        if (Balance == newBalance) return;
        
        var oldBalance = Balance;
        Balance = newBalance;
        _domainEvents.Add(new UserBalanceChangedEvent(Id, oldBalance, newBalance, reason));
    }

    public void UpdateProfile(string? newImageUrl)
    {
        if (ImageUrl == newImageUrl) return;

        var oldImageUrl = ImageUrl;
        ImageUrl = newImageUrl;
        _domainEvents.Add(new UserProfileUpdatedEvent(Id, oldImageUrl, newImageUrl));
    }

    public void UpdateRole(string newRole)
    {
        var oldRole = CurrentRole ?? "None";
        CurrentRole = newRole;
        _domainEvents.Add(new UserRoleChangedEvent(Id, oldRole, newRole));
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}