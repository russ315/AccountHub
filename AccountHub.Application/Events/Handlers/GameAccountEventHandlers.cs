using AccountHub.Domain.Entities;
using AccountHub.Domain.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AccountHub.Application.Events.Handlers;

public class GameAccountSoldEventHandler : IEventHandler<GameAccountSoldEvent>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ILogger<GameAccountSoldEventHandler> _logger;

    public GameAccountSoldEventHandler(
        UserManager<UserEntity> userManager,
        ILogger<GameAccountSoldEventHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task HandleAsync(GameAccountSoldEvent domainEvent)
    {
        _logger.LogInformation("Handling GameAccountSoldEvent: Account {GameAccountId} sold from {PreviousOwnerId} to {NewOwnerId}",
            domainEvent.GameAccountId, domainEvent.PreviousOwnerId, domainEvent.NewOwnerId);

        try
        {
            // Get seller
            var seller = await _userManager.FindByIdAsync(domainEvent.PreviousOwnerId);
            if (seller == null)
            {
                _logger.LogWarning("Seller {SellerId} not found", domainEvent.PreviousOwnerId);
                return;
            }

            // Update seller's balance
            seller.UpdateBalance(
                seller.Balance + domainEvent.Price,
                $"Sale of Game Account #{domainEvent.GameAccountId}"
            );

            // Save changes
            var result = await _userManager.UpdateAsync(seller);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update seller balance: {Errors}", errors);
                return;
            }
            
            _logger.LogInformation("Updated seller {SellerId} balance", domainEvent.PreviousOwnerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating seller balance for GameAccountSoldEvent");
        }
    }
}

public class GameAccountCreatedEventHandler : IEventHandler<GameAccountCreatedEvent>
{
    private readonly ILogger<GameAccountCreatedEventHandler> _logger;

    public GameAccountCreatedEventHandler(ILogger<GameAccountCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(GameAccountCreatedEvent domainEvent)
    {
        _logger.LogInformation("New game account created: {GameAccountId} by seller {SellerId} with price {Price}",
            domainEvent.GameAccountId, domainEvent.SellerId, domainEvent.Price);
        
        // Add any additional logic here, such as:
        // - Sending notifications
        // - Updating statistics
        // - Triggering third-party integrations
        
        return Task.CompletedTask;
    }
} 
public class GameAccountCredentialHandler:IEventHandler<GameAccountCredentialsUpdatedEvent>
{
    private readonly ILogger<GameAccountSoldEventHandler> _logger;

    public GameAccountCredentialHandler(
        ILogger<GameAccountSoldEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(GameAccountCredentialsUpdatedEvent domainEvent)
    {
        _logger.LogInformation("Time:{OccurredOn}.Handling GameAccountCredentialsUpdatedEvent:  Account {GameAccountId} has credentials:{Action}",
            domainEvent.OccurredOn,domainEvent.GameAccountId,domainEvent.Action);
        
        return Task.CompletedTask;
       
    }
}