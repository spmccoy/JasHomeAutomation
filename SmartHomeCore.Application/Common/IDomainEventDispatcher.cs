using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Application.Common;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(DomainEvent domainEvent);
    Task DispatchAsync(IEnumerable<DomainEvent> domainEvents);
}
