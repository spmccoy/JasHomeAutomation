using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Application.EventHandlers;

public class DomainErrorHandler : IDomainEventHandler<DomainErrorEvent>
{
    private readonly ILogger<DomainErrorHandler> _logger;

    public DomainErrorHandler(ILogger<DomainErrorHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(DomainErrorEvent domainEvent)
    {
        _logger.LogError(domainEvent.ErrorMessage);
        
        return Task.CompletedTask;
    }
}