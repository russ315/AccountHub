namespace AccountHub.Domain.Events;

public class UserBalanceChangedEvent : DomainEvent
{
    public string UserId { get; }
    public decimal OldBalance { get; }
    public decimal NewBalance { get; }
    public string Reason { get; }

    public UserBalanceChangedEvent(string userId, decimal oldBalance, decimal newBalance, string reason)
    {
        UserId = userId;
        OldBalance = oldBalance;
        NewBalance = newBalance;
        Reason = reason;
    }
}

public class UserProfileUpdatedEvent : DomainEvent
{
    public string UserId { get; }
    public string? OldImageUrl { get; }
    public string? NewImageUrl { get; }

    public UserProfileUpdatedEvent(string userId, string? oldImageUrl, string? newImageUrl)
    {
        UserId = userId;
        OldImageUrl = oldImageUrl;
        NewImageUrl = newImageUrl;
    }
}

public class UserRoleChangedEvent : DomainEvent
{
    public string UserId { get; }
    public string OldRole { get; }
    public string NewRole { get; }

    public UserRoleChangedEvent(string userId, string oldRole, string newRole)
    {
        UserId = userId;
        OldRole = oldRole;
        NewRole = newRole;
    }
} 