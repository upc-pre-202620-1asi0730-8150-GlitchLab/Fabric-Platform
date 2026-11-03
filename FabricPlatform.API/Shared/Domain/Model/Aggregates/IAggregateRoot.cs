using FabricPlatform.API.Shared.Domain.Model.Events;

namespace FabricPlatform.API.Shared.Domain.Model.Aggregates;

public interface IAggregateRoot
{
    IReadOnlyCollection<IEvent> DomainEvents { get; }
    void ClearDomainEvents();
}