using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HomeAssistantGenerated;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemonApps.DomainEntities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

/// <summary>
/// Provides functionality for managing and operating MQTT Select entities in a Home Assistant environment.
/// This service handles the creation and registration of MQTT Select entities by leveraging the
/// IMqttEntityManager interface and associated entity-related methods.
/// </summary>
public class MqttSelectService(IMqttEntityManager entityManager, Entities entities, ILogger<MqttSelectService> logger)
    : IMqttSelectService
{
    /// <inheritdoc/>
    public async Task CreateAllAsync()
    {
        foreach (var s in GetSelects())
        {
            await CreateAsync(s);
        }
    }

    /// <inheritdoc/>
    public async Task RegisterAllAsync()
    {
        foreach (var s in GetSelects())
        {
            await RegisterAsync(s);
        }
    }

    /// <summary>
    /// Creates an MQTT Select entity in the Home Assistant environment using the provided configuration.
    /// </summary>
    /// <param name="mqttSelect">The MQTT Select configuration that includes unique ID, display name, and state handlers.</param>
    /// <returns>A task that represents the asynchronous creation operation.</returns>
    private async Task CreateAsync(MqttSelect mqttSelect)
    {
        // Create entity
        await entityManager.CreateAsync(
            mqttSelect.Id,
            new EntityCreationOptions(Name: mqttSelect.DisplayName, DeviceClass: HaEntityType.Switch.ToString()),
            new
            {
                options = mqttSelect.StateHandlers.Select(s => s.Key).ToArray()
            });
    }

    /// <summary>
    /// Registers an MQTT Select entity, subscribes to its state changes, and assigns the corresponding state handlers.
    /// </summary>
    /// <param name="mqttSelect">The MQTT Select entity to be registered, which contains the state handlers and configurations.</param>
    /// <returns>A task that represents the asynchronous operation of registering the MQTT Select entity.</returns>
    private async Task RegisterAsync(MqttSelect mqttSelect)
    {
        (await entityManager.PrepareCommandSubscriptionAsync(mqttSelect.Id).ConfigureAwait(false))
            // ReSharper disable once AsyncVoidLambda
            .Subscribe(async state =>
            {
                await entityManager.SetStateAsync(mqttSelect.Id, state).ConfigureAwait(false);

                if (mqttSelect.StateHandlers.TryGetValue(state, out var handler))
                {
                    handler();
                }
                else
                {
                    logger.LogError("Unable to find handler for state {State} in select {Id}", state, mqttSelect.Id);
                }
            });
    }

    /// <summary>
    /// Retrieves all instances of types that inherit from MqttSelect and are defined in the current assembly.
    /// </summary>
    /// <returns>An array of MqttSelect objects created through reflection.</returns>
    private MqttSelect[] GetSelects()
    {
        var mqttSwitchType = typeof(MqttSelect);

        var switchTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(mqttSwitchType));

        return switchTypes.Select(type => (MqttSelect)Activator.CreateInstance(type, entities)!).ToArray();
    }
}