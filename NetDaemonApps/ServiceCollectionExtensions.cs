using Microsoft.Extensions.DependencyInjection;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

namespace NetDaemonApps;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ILightService, LightService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ISensorService, SensorService>();
        services.AddScoped<ILightService, LightService>();
        services.AddScoped<ISunService, SunService>();
        services.AddScoped<IShawnRoomService, ShawnRoomService>();
        services.AddScoped<IMainRoomService, MainRoomService>();
        services.AddScoped<IHouseService, HouseService>();
        services.AddScoped<ICameraService, CameraService>();
        return services;
    }
}