using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps;

/// <summary>
/// Represents an application-level subscriber for managing MQTT entity services, including
/// switches, selects, and scenes. The class initializes these services when the application starts.
/// </summary>
[NetDaemonApp(Id = $"{nameof(MqttEntityStateManager)}")]
public class MqttEntityStateManager : IAsyncInitializable
{
    private readonly IMqttEntityManager _mqttEntityManager;
    private readonly IEnumerable<MqttEntity> _mqttEntities;
    private readonly ILogger<MqttEntityStateManager> _logger;

    /// <summary>
    /// Represents an application-level subscriber for managing MQTT entity services, including
    /// switches, selects, and scenes. The class initializes these services when the application starts.
    /// </summary>
    public MqttEntityStateManager(
        IMqttEntityManager mqttEntityManager,
        IEnumerable<MqttEntity> mqttEntities,
        ILogger<MqttEntityStateManager> logger)
    {
        _mqttEntityManager = mqttEntityManager;
        _mqttEntities = mqttEntities;
        _logger = logger;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        foreach (var mqttEntity in _mqttEntities)
        {
            await HandleMqttEntityState(mqttEntity);
        }
    }

    private async Task HandleMqttEntityState(MqttEntity mqttEntity)
    {
        (await _mqttEntityManager.PrepareCommandSubscriptionAsync(mqttEntity.Id).ConfigureAwait(false))
            // ReSharper disable once AsyncVoidLambda
            .Subscribe(async state =>
            {
                _logger.LogInformation("{Name} has changed state to {State}", mqttEntity.DisplayName, state);
                await _mqttEntityManager.SetStateAsync(mqttEntity.Id, state).ConfigureAwait(false);
                mqttEntity.HandleStateChange(state);
            });
    }
}