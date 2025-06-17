using Integrations.HomeDevices.HomeAssistantGenerated;
using Microsoft.Extensions.DependencyInjection;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Infrastructure.Integrations.HomeDevices;

namespace SmartHomeCore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<ButtonEntities>();
        services.AddScoped<IHomeAutomationClient, HomeAutomationClient>();

        return services;
    }
}