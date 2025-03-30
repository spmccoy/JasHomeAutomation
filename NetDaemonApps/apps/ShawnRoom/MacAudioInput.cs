using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp(Id = $"{nameof(ShawnRoom)}_{nameof(MacAudioInput)}")]
public class MacAudioInput
{
    private readonly Entities _entities;
    private readonly ILogger _logger;

    public MacAudioInput(Entities entities, ILogger<MacAudioInput> logger)
    {
        _entities = entities;
        _logger = logger;

        entities.Sensor.ShawnsMacbookProActiveAudioInput
            .StateChanges()
            .Subscribe(s => ProcessState(s));
    }

    private LightAttributes? LastKnownLightAttributes { get; set; }

    private void ProcessState(StateChange<SensorEntity, EntityState<SensorAttributes>> stateChange)
    {
        var newState = stateChange.New?.State;
        var oldState = stateChange.Old?.State;
        
        _logger.LogInformation("State changed from {oldState} to {newState}", oldState, newState);
        
        if (newState == HaCommonState.Active.ToString())
        {
            HandleActiveState();
        }
        else if (newState == HaCommonState.InActive.ToString())
        {
            HandleInactiveState();
        }
    }

    private void HandleActiveState()
    {
        LastKnownLightAttributes = _entities.Light.HuePlayRight.Attributes;
        _entities.Light.HuePlayRight.TurnOn(colorName: "Red");
    }

    private void HandleInactiveState()
    {
        if (LastKnownLightAttributes == null)
        {
            _logger.LogWarning("The last known light attributes did not save. Turning light off instead.");
            _entities.Light.HuePlayRight.TurnOff();
        }
        else
        {
            _entities.Light.HuePlayRight.TurnOn(hsColor: LastKnownLightAttributes.HsColor);
        }
    }
}