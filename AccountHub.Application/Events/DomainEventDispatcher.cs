using AccountHub.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AccountHub.Application.Events;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IDomainEvent> events);
}

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task DispatchEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            _logger.LogInformation("Dispatching domain event: {EventType}", @event.EventType);
            
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (dynamic? handler in handlers)
            {
                try
                {
                    await handler?.HandleAsync((dynamic)@event)!;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling domain event {EventType}", @event.EventType);
                }
            }
        }
    }
} 