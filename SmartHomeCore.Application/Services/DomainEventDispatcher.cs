using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Application.Services;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(DomainEvent domainEvent)
    {
        var domainEventType = domainEvent.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEventType);
        
        await using var scope = _serviceProvider.CreateAsyncScope();
        dynamic? handler = scope.ServiceProvider.GetService(handlerType);
        
        if (handler != null)
        {
            await handler.HandleAsync((dynamic)domainEvent);
        }
    }

    public async Task DispatchAsync(IEnumerable<DomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await DispatchAsync(domainEvent);
        }
    }
}