using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HomeAssistantGenerated;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemonApps.DomainEntities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

/// <summary>
/// Service responsible for creating and registering MQTT-based switches
/// within the NetDaemon framework. Provides functionality to manage
/// MQTT switches and interact with the Home Assistant environment.
/// </summary>
public class MqttSwitchService(IMqttEntityManager entityManager, Entities entities) : IMqttSwitchService
{
    /// <inheritdoc/>
    public async Task CreateAllAsync()
    {
        foreach (var s in GetSwitches())
        {
            await CreateAsync(s);
        }
    }

    /// <inheritdoc/>
    public async Task RegisterAllAsync()
    {
        foreach (var s in GetSwitches())
        {
            await RegisterAsync(s);
        }
    }

    /// <summary>
    /// Creates and registers a switch within the MQTT system.
    /// </summary>
    /// <param name="mqttSwitch">The MQTT switch to be created, containing its ID and display name.</param>
    /// <returns>A task that represents the asynchronous operation of creating the MQTT switch.</returns>
    private async Task CreateAsync(MqttSwitch mqttSwitch)
    {
        await entityManager.CreateAsync(
            mqttSwitch.Id,
            new EntityCreationOptions(Name: mqttSwitch.DisplayName, DeviceClass: HaEntityType.Switch.ToString()));
    }

    /// <summary>
    /// Registers an MQTT-based switch within the NetDaemon framework by subscribing
    /// to its command topic and handling state changes accordingly.
    /// </summary>
    /// <param name="mqttSwitch">The MQTT switch to register, containing its unique identity and behavior logic.</param>
    /// <returns>A task that represents the asynchronous operation of registering the switch.</returns>
    private async Task RegisterAsync(MqttSwitch mqttSwitch)
    {
        (await entityManager.PrepareCommandSubscriptionAsync(mqttSwitch.Id).ConfigureAwait(false))
            // ReSharper disable once AsyncVoidLambda
            .Subscribe(async state =>
            {
                await entityManager.SetStateAsync(mqttSwitch.Id, state).ConfigureAwait(false);
                mqttSwitch.HandleStateChange(state);
            });
    }

    /// <summary>
    /// Retrieves an array of MqttSwitch instances by discovering and instantiating
    /// all non-abstract classes that derive from the MqttSwitch base class within the current assembly.
    /// </summary>
    /// <returns>An array of instances of MqttSwitch-derived types.</returns>
    private MqttSwitch[] GetSwitches()
    {
        var mqttSwitchType = typeof(MqttSwitch);

        var switchTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(mqttSwitchType));

        return switchTypes.Select(type => (MqttSwitch)Activator.CreateInstance(type, entities)!).ToArray();
    }
}