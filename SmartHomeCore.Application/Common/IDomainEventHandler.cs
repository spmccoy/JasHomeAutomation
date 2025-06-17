using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Application.Common;

public interface IDomainEventHandler<T> where T : DomainEvent
{
    Task HandleAsync(T domainEvent);
}
