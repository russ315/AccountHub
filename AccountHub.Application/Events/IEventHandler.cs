using AccountHub.Domain.Events;

namespace AccountHub.Application.Events;

public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
} 