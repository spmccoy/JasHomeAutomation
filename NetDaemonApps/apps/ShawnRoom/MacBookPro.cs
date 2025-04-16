using Domain.Entities;
using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.apps.ShawnRoom.Devices;

[NetDaemonApp]
public class MacBookPro
{
    private readonly Entities _entities;
    private readonly ILogger _logger;

    public MacBookPro(Entities entities, ILogger<MacBookPro> logger)
    {
        _entities = entities;
        _logger = logger;

        entities.Sensor.ShawnsMacbookProActiveAudioInput
            .StateChanges()
            .Subscribe(ProcessStateChange);
    }

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
        
        if (newState == HaState.Active)
        {
            _entities.Switch.ShawnroomDndNetdaemon.TurnOn();
        }
        else if (newState == HaState.InActive)
        {
            _entities.Switch.ShawnroomDndNetdaemon.TurnOff();
        }
    }
}