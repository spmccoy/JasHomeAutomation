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
    private readonly Entities _entities;

    /// <summary>
    /// Represents an application-level subscriber for managing MQTT entity services, including
    /// switches, selects, and scenes. The class initializes these services when the application starts.
    /// </summary>
    public MqttEntityStateManager(
        IMqttEntityManager mqttEntityManager,
        IEnumerable<MqttEntity> mqttEntities,
        Entities entities)
    {
        _mqttEntityManager = mqttEntityManager;
        _mqttEntities = mqttEntities;
        _entities = entities;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var sun = new Sun(_entities.Sun.Sun, DateTime.UtcNow);
        
        foreach (var mqttEntity in _mqttEntities)
        {
            if (mqttEntity.InitialValue is not null)
            {
                await _mqttEntityManager.SetStateAsync(mqttEntity.Id, mqttEntity.InitialValue).ConfigureAwait(false);   
            }
            
            await HandleMqttEntityState(mqttEntity);
        }
    }

    private async Task HandleMqttEntityState(MqttEntity mqttEntity)
    {
        (await _mqttEntityManager.PrepareCommandSubscriptionAsync(mqttEntity.Id).ConfigureAwait(false))
            // ReSharper disable once AsyncVoidLambda
            .Subscribe(async state =>
            {
                switch (mqttEntity)
                {
                    case MqttSwitch { PersistState: false }:
                        await _mqttEntityManager.SetStateAsync(mqttEntity.Id, MqttSwitch.Unknown).ConfigureAwait(false);
                        break;
                    default:
                        await _mqttEntityManager.SetStateAsync(mqttEntity.Id, state).ConfigureAwait(false);
                        break;
                }
                
                mqttEntity.HandleStateChange(state);
            });
    }
}