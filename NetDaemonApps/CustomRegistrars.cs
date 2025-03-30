using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class CustomRegistrars
{
    public static IServiceCollection AddMqttEntities(this IServiceCollection services)
    {
        const string containingNamespace = $"{nameof(NetDaemonApps)}.{nameof(NetDaemonApps.apps)}";
        var baseType = typeof(MqttEntity);
        var assembly = Assembly.Load(nameof(NetDaemonApps));

        var mqttEntityTypes = assembly.GetTypes()
            .Where(t =>
                t.IsSubclassOf(baseType) &&
                !t.IsAbstract &&
                t.Namespace!.StartsWith(containingNamespace));

        foreach (var type in mqttEntityTypes)
        {
            services.AddScoped(baseType, type);
        }

        return services;
    }
}