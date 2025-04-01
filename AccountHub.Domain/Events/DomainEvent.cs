namespace AccountHub.Domain.Events;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; }
    public string EventType { get; }

    protected DomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
        EventType = GetType().Name;
    }
} 