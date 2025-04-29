using Domain.Entities;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MacBookPro
{
    private readonly IPersonService _personService;
    private readonly SwitchEntities _switches;
    private readonly ILogger _logger;

    public MacBookPro(IPersonService personService, SensorEntities sensors, SwitchEntities switches, ILogger<MacBookPro> logger)
    {
        _personService = personService;
        _switches = switches;
        _logger = logger;

        sensors.ShawnsMacbookProActiveAudioInput
            .StateChanges()
            .Subscribe(ProcessStateChange);
    }

    private void ProcessStateChange(StateChange<SensorEntity, EntityState<SensorAttributes>> stateChange)
    {
        var newState = stateChange.New?.State;
        var oldState = stateChange.Old?.State;
        
        _logger.LogInformation("State changed from {oldState} to {newState}", oldState, newState);

        if (!_personService.ShawnHome)
        {
            _logger.LogDebug("Short circuit, shawn is not home");
            return;
        }
        
        if (newState == HaState.Active)
        {
            _switches.ShawnroomDndNetdaemon.TurnOn();
        }
        else if (newState == HaState.InActive)
        {
            _switches.ShawnroomDndNetdaemon.TurnOff();
        }
    }
}