using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.AreaEntity;

namespace SmartHomeCore.Application.EventHandlers;

public class AreaStateChangedEventHandler(IHomeAutomationClient homeAutomationClient, ILogger<AreaStateChangedEventHandler> logger) 
    : IDomainEventHandler<AreaStateChangedEvent>
{
    public Task HandleAsync(AreaStateChangedEvent domainEvent)
    {
        logger.LogDebug(EventHandlerConstants.StateChangeLog, domainEvent.Name, domainEvent.OldState, domainEvent.NewState);

        if (domainEvent.Scene is null)
        {
            logger.LogDebug(
                "{NewState} for area {AreaName} does not have a scene associated with it. Ignoring scene change.",
                domainEvent.NewState, 
                domainEvent.Name);
            return Task.CompletedTask;
        }

        homeAutomationClient.ActivateScene(domainEvent.Scene.Id);
        return Task.CompletedTask;
    }
}