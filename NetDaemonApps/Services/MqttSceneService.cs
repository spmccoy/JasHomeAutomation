using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

/// <summary>
/// Service responsible for managing MQTT-based scenes.
/// </summary>
/// <remarks>
/// This service provides operations to create and register scenes using MQTT.
/// It interacts with the MQTT entity manager to manage the lifecycle of scenes.
/// </remarks>
public class MqttSceneService(IMqttEntityManager entityManager) : IMqttSceneService
{
    /// <inheritdoc/>
    public async Task CreateAllAsync()
    {
        foreach (var s in GetScenes())
        {
            await CreateAsync(s);
        }
    }

    /// <inheritdoc/>
    public async Task RegisterAllAsync()
    {
        foreach (var s in GetScenes())
        {
            await RegisterAsync(s);
        }
    }

    /// <summary>
    /// Asynchronously creates an MQTT scene using the specified scene configuration.
    /// </summary>
    /// <param name="scene">The scene to be created, including its group name, entity name, and display name.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CreateAsync(MqttScene scene)
    {
        await entityManager.CreateAsync(scene.Id, new EntityCreationOptions(Name: scene.DisplayName));
    }

    /// <summary>
    /// Registers an MQTT scene and sets up a subscription for handling state changes.
    /// </summary>
    /// <param name="mqttScene">The MQTT scene to be registered.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task RegisterAsync(MqttScene mqttScene)
    {
        (await entityManager.PrepareCommandSubscriptionAsync(mqttScene.Id).ConfigureAwait(false))
            // ReSharper disable once AsyncVoidLambda
            .Subscribe(async state =>
            {
                await entityManager.SetStateAsync(mqttScene.Id, state).ConfigureAwait(false);
            });
    }

    /// <summary>
    /// Retrieves all MQTT scenes available in the current assembly.
    /// </summary>
    /// <returns>An array of <see cref="MqttScene"/> representing the available scenes.</returns>
    private MqttScene[] GetScenes()
    {
        var mqttSwitchType = typeof(MqttScene);

        var switchTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(mqttSwitchType));

        return switchTypes.Select(type => (MqttScene)Activator.CreateInstance(type)!).ToArray();
    }
}