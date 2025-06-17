using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.HouseEntity;

namespace SmartHomeCore.Application.UseCases;

public abstract class UseCase(House house, IDomainEventDispatcher domainEventDispatcher)
{
    public async Task HandleAsync()
    {
        await ExecuteCoreLogicAsync();

        var events = house.CollectAndClearAllDomainEvents();
        await domainEventDispatcher.DispatchAsync(events);
    }

    protected virtual Task ExecuteCoreLogicAsync()
    {
        throw new NotImplementedException();
    }
}