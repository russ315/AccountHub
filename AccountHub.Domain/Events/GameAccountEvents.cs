using System.Text.Json;
using AccountHub.Domain.Models;

namespace AccountHub.Domain.Events;

public class GameAccountCreatedEvent : DomainEvent
{
    public long GameAccountId { get; }
    public string SellerId { get; }
    public decimal Price { get; }
    public AccountStatus Status { get; }

    public GameAccountCreatedEvent(long gameAccountId, string sellerId, decimal price, AccountStatus status)
    {
        GameAccountId = gameAccountId;
        SellerId = sellerId;
        Price = price;
        Status = status;
    }
}

public class GameAccountUpdatedEvent : DomainEvent
{
    public long GameAccountId { get; }

    public GameAccountUpdatedEvent(long gameAccountId)
    {
        GameAccountId = gameAccountId;
    }
}

public class GameAccountSoldEvent : DomainEvent
{
    public long GameAccountId { get; }
    public string PreviousOwnerId { get; }
    public string NewOwnerId { get; }
    public decimal Price { get; }

    public GameAccountSoldEvent(long gameAccountId, string previousOwnerId, string newOwnerId, decimal price)
    {
        GameAccountId = gameAccountId;
        PreviousOwnerId = previousOwnerId;
        NewOwnerId = newOwnerId;
        Price = price;
    }
}

public class GameAccountStatusChangedEvent : DomainEvent
{
    public long GameAccountId { get; }
    public AccountStatus OldStatus { get; }
    public AccountStatus NewStatus { get; }

    public GameAccountStatusChangedEvent(long gameAccountId, AccountStatus oldStatus, AccountStatus newStatus)
    {
        GameAccountId = gameAccountId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public class GameAccountCredentialsUpdatedEvent : DomainEvent
{
    public long GameAccountId { get; }
    public string CredentialType { get; }
    public string Action { get; }

    public GameAccountCredentialsUpdatedEvent(long gameAccountId, string credentialType, string action)
    {
        GameAccountId = gameAccountId;
        CredentialType = credentialType;
        Action = action;
    }
} 