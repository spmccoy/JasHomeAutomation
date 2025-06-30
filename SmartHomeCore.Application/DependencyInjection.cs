using Microsoft.Extensions.DependencyInjection;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Application.EventHandlers;
using SmartHomeCore.Application.Services;
using SmartHomeCore.Application.UseCases;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        HouseConfig.Register(services);
        RegisterAllDomainEventHandlers(services);
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        RegisterAllUseCases(services);
        
        return services;
    }
    
    private static void RegisterAllDomainEventHandlers(IServiceCollection services)
    {
        var assembly = typeof(DomainErrorHandler).Assembly;
        
        var handlerTypes = assembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && 
                          i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)));
        
        foreach (var handlerType in handlerTypes)
        {
            var handlerInterface = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && 
                            i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>));
            
            services.AddScoped(handlerInterface, handlerType);
        }
    }

    private static void RegisterAllUseCases(IServiceCollection services)
    {
        // Get all types that inherit from UseCase
        var useCaseTypes = typeof(UseCase).Assembly
            .GetTypes()
            .Where(t => t.IsClass 
                        && !t.IsAbstract 
                        && t.IsSubclassOf(typeof(UseCase)));

        // Register each use case as transient
        foreach (var useCaseType in useCaseTypes)
        {
            services.AddTransient(useCaseType);
        }

    }
}