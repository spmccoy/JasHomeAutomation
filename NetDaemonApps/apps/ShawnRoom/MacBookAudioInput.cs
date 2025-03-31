using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MacBookAudioInput
{
    private readonly Entities _entities;
    private readonly ILogger _logger;

    public MacBookAudioInput(Entities entities, ILogger<MacBookAudioInput> logger)
    {
        _entities = entities;
        _logger = logger;

        entities.Sensor.ShawnsMacbookProActiveAudioInput
            .StateChanges()
            .Subscribe(ProcessStateChange);
    }

    private LightAttributes? LastKnownLightAttributes { get; set; }

    private void ProcessStateChange(StateChange<SensorEntity, EntityState<SensorAttributes>> stateChange)
    {
        var newState = stateChange.New?.State;
        var oldState = stateChange.Old?.State;
        var shawn = new Person(_entities.Person.Shawn.State);
        
        _logger.LogInformation("State changed from {oldState} to {newState}", oldState, newState);

        if (!shawn.IsHome)
        {
            _logger.LogDebug("Short circuit, shawn is not home");
            return;
        }
        
        if (newState == HaCommonState.Active.ToString())
        {
            _entities.Switch.NetdaemonShawnroomDndSwitch.TurnOn();
        }
        else if (newState == HaCommonState.InActive.ToString())
        {
            _entities.Switch.NetdaemonShawnroomDndSwitch.TurnOff();
        }
    }
}