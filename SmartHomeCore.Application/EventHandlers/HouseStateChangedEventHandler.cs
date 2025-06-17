using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.HouseEntity;

namespace SmartHomeCore.Application.EventHandlers;

public class HouseStateChangedEventHandler(
    ILogger<HouseStateChangedEventHandler> logger, 
    IHomeAutomationClient homeAutomationClient) 
    : IDomainEventHandler<HouseStateChangedEvent>
{
    public async Task HandleAsync(HouseStateChangedEvent domainEvent)
    {
        logger.LogDebug("House state changed: {OldState} -> {NewState}", domainEvent.OldState, domainEvent.NewState);

        if (domainEvent.NewState.IsSecure())
        {
            await homeAutomationClient.SecureAllEntryPointsAsync();
            logger.LogInformation("House has been secured.");
        }

        if (domainEvent.NewState.IsUnsecure())
        {
            logger.LogInformation("House is unsecured.");
        }

        if (domainEvent.NewState.IsSleep())
        {
            await homeAutomationClient.SecureAllEntryPointsAsync();
            logger.LogInformation("House is sleeping.");
        }
        
        if (domainEvent.NewState.IsEveryoneAway())
        {
            await homeAutomationClient.SecureAllEntryPointsAsync();
            logger.LogInformation("House set to away.");
        }
    }
}