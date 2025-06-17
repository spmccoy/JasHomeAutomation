using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.PersonEntity;

namespace SmartHomeCore.Application.EventHandlers;

public class PersonStateChangedEventHandler(ILogger<PersonStateChangedEventHandler> logger, IHomeAutomationClient homeAutomationClient) 
    : IDomainEventHandler<PersonStateChangedEvent>
{
    public async Task HandleAsync(PersonStateChangedEvent domainEvent)
    {
        logger.LogDebug("{Name} state changed: {OldState} -> {NewState}", domainEvent.PersonName, domainEvent.OldState, domainEvent.NewState);

        if (domainEvent.NewState.IsAway())
        {
            await homeAutomationClient.CloseGarageAsync();
        }

        if (domainEvent.NewState.IsHome())
        {
            await homeAutomationClient.OpenGarageAsync();
        }
    }
}