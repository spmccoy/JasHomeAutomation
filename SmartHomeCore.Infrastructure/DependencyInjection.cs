using Integrations.HomeDevices.HomeAssistantGenerated;
using Microsoft.Extensions.DependencyInjection;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Infrastructure.Common;
using SmartHomeCore.Infrastructure.Integrations.HomeDevices;
using SmartHomeCore.Infrastructure.Integrations.HomeDevices.Rooms;
using SmartHomeCore.Infrastructure.Integrations.Notifications;

namespace SmartHomeCore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<ButtonEntities>();
        services.AddScoped<IHomeAutomationClient, HomeAutomationClient>();
        services.AddScoped<INotificationClient, NotificationClient>();
        services.AddScoped<IHomeAutomationClient, HomeAutomationClient>();
        
        services.AddScoped<CoverEntities>();
        services.AddScoped<LockEntities>();
        services.AddScoped<LightEntities>();

        services.AddHomeAssistantGenerated();

        AddRooms(services);

        return services;
    }

    private static void AddRooms(IServiceCollection services)
    {
        services.AddScoped<IGarage, Garage>();
        services.AddScoped<IMainRoom, MainRoom>();
    }
}