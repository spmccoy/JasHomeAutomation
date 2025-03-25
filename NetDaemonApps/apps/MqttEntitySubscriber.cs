using System.Threading;
using System.Threading.Tasks;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps;

/// <summary>
/// Represents an application-level subscriber for managing MQTT entity services, including
/// switches, selects, and scenes. The class initializes these services when the application starts.
/// </summary>
[NetDaemonApp(Id = $"{nameof(MqttEntitySubscriber)}")]
public class MqttEntitySubscriber(
    IMqttSwitchService mqttSwitchService,
    IMqttSelectService mqttSelectService,
    IMqttSceneService mqttSceneService) : IAsyncInitializable
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await mqttSwitchService.RegisterAllAsync();
        await mqttSelectService.RegisterAllAsync();
        await mqttSceneService.RegisterAllAsync();
    }
}