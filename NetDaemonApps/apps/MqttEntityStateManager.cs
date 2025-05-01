using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MqttEntities.Common;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.HassModel.Entities;
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

    /// <summary>
    /// Represents an application-level subscriber for managing MQTT entity services, including
    /// switches, selects, and scenes. The class initializes these services when the application starts.
    /// </summary>
    public MqttEntityStateManager(
        IMqttEntityManager mqttEntityManager,
        Entities entities,
        IHaContext haContext)
    {
        _mqttEntityManager = mqttEntityManager;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var mqttEntities = MqttEntityManager.GetSubtypeInstancesOfMqttEntity();
        foreach (var mqttEntity in mqttEntities)
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
                await _mqttEntityManager.SetStateAsync(mqttEntity.Id, state).ConfigureAwait(false);
            });
    }
}