using AccountHub.Domain.Events;
using Microsoft.Extensions.Logging;

namespace AccountHub.Application.Events.Handlers;

public class GameAccountStatusChangeHandler : IEventHandler<GameAccountStatusChangedEvent>
{
    private readonly ILogger<GameAccountStatusChangeHandler> _logger;

    public GameAccountStatusChangeHandler(ILogger<GameAccountStatusChangeHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task HandleAsync(GameAccountStatusChangedEvent domainEvent)
    {
        _logger.LogInformation(
            "Game account status changed - ID: {GameAccountId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}",
            domainEvent.GameAccountId,
            domainEvent.OldStatus,
            domainEvent.NewStatus);

        // Additional business logic based on status change
        // For example:
        // - If status changed to Available, notify potential buyers
        // - If status changed to Sold, update statistics
        // - If status changed to Unavailable, notify relevant parties
        
        return Task.CompletedTask;
    }
} 