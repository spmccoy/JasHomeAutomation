using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.Common;
using SmartHomeCore.Domain.HouseEntity;

namespace SmartHomeCore.Application.UseCases;

public abstract class UseCase(House house, IDomainEventDispatcher domainEventDispatcher, ILogger logger)
{
    public async Task HandleAsync()
    {
        try
        {
            logger.LogInformation("Executing core logic for {UseCase}", GetType().Name);
            await ExecuteCoreLogicAsync();

            var events = house.CollectAndClearAllDomainEvents();
            logger.LogDebug("Dispatching {EventCount} domain events", events.Count);
            await domainEventDispatcher.DispatchAsync(events);
            logger.LogInformation("Successfully completed {UseCase}", GetType().Name);
        }
        catch (EntityNotFoundException entityNotFound)
        {
            logger.LogWarning(entityNotFound, "Entity not found error in {UseCase}: {ErrorMessage}", 
                GetType().Name, entityNotFound.Message);
        }
    }

    protected virtual Task ExecuteCoreLogicAsync()
    {
        throw new NotImplementedException();
    }
}