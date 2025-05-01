using System.Reactive.Concurrency;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MacBookPro
{
    public MacBookPro(IPersonService personService, BinarySensorEntities binarySensors, SwitchEntities switches, IScheduler scheduler, ILogger<MacBookPro> logger)
    {
        // when audio input is on for half a second
        binarySensors.ShawnsMacbookProAudioInputInUse
            .StateChanges()
            .WhenStateIsFor(s => s?.State == HaState.On, TimeSpan.FromMilliseconds(500), scheduler)
            .Subscribe(_ =>
            {
                switches.ShawnroomDndNetdaemon.TurnOn();
            });
        
        // when audio input is off for half a second
        binarySensors.ShawnsMacbookProAudioInputInUse
            .StateChanges()
            .WhenStateIsFor(s => s?.State == HaState.Off, TimeSpan.FromMilliseconds(500), scheduler)
            .Subscribe(_ =>
            {
                switches.ShawnroomDndNetdaemon.TurnOff();
            });
    }
}