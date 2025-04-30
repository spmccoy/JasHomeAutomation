using Domain.Entities;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MacBookPro
{
    public MacBookPro(IPersonService personService, BinarySensorEntities binarySensors, SwitchEntities switches, ILogger<MacBookPro> logger)
    {
        binarySensors.ShawnsMacbookProAudioInputInUse
            .StateChanges()
            .Subscribe(stateChange =>
            {
                var newState = stateChange.New?.State;
                var oldState = stateChange.Old?.State;
        
                logger.LogInformation("State changed from {oldState} to {newState}", oldState, newState);

                if (!personService.ShawnHome)
                {
                    logger.LogDebug("Short circuit, shawn is not home");
                    return;
                }
        
                switch (newState)
                {
                    case HaState.On:
                        switches.ShawnroomDndNetdaemon.TurnOn();
                        break;
                    
                    case HaState.Off:
                        switches.ShawnroomDndNetdaemon.TurnOff();
                        break;
                }
            });
    }
}