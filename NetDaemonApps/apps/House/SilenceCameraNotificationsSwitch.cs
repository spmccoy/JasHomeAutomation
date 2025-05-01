using System.Reactive.Concurrency;
using MqttEntities.Models;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.House;
[NetDaemonApp]
public class SilenceCameraNotificationsSwitch
{
    private const int TurnBackOnInMinutes = 30;
    
    private readonly SwitchEntities _switches;
    private readonly IScheduler _scheduler;
    
    private IDisposable? _schedulerSubscription;
    
    public SilenceCameraNotificationsSwitch(SwitchEntities switches, IScheduler scheduler, SelectEntities selects)
    {
        _switches = switches;
        _scheduler = scheduler;
        switches.HouseCameraNotificationsNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.On)
            .Subscribe(_ => Helpers.CancelSchedule(_schedulerSubscription));
        
        switches.HouseCameraNotificationsNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.Off)
            .Subscribe(_ =>
            {
                Helpers.CancelSchedule(_schedulerSubscription);
                _schedulerSubscription = _scheduler.Schedule(
                    TimeSpan.FromMinutes(TurnBackOnInMinutes), 
                    () =>
                    {
                        if (selects.HouseStateNetdaemon.State != RoomStates.HomeUnsecured.ToString())
                        {
                            _switches.HouseCameraNotificationsNetdaemon.TurnOn();
                        }
                    });
            });
    }
}