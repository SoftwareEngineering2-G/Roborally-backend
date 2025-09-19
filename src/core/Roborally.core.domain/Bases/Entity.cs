using FastEndpoints;

namespace Roborally.core.domain.Bases;

public abstract class Entity {
    private List<IEvent>? _domainEvents;
    public List<IEvent>? DomainEvents => _domainEvents;

    protected void AddDomainEvent(IEvent domainEvent) {
        _domainEvents = _domainEvents ?? [];
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() {
        _domainEvents?.Clear();
    }
}