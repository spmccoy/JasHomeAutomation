using System.Reactive.Concurrency;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.House;
[NetDaemonApp]
public class SilenceCameraNotificationsSwitch
{
    private const int TurnBackOnInMinutes = 30;
    
    private readonly SwitchEntities _switches;
    private readonly IScheduler _scheduler;
    
    private IDisposable? _schedulerSubscription;
    
    public SilenceCameraNotificationsSwitch(SwitchEntities switches, IScheduler scheduler)
    {
        _switches = switches;
        _scheduler = scheduler;
        switches.HouseCameraNotificationsNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.On)
            .Subscribe(_ => HandleOn());
        
        switches.HouseCameraNotificationsNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.Off)
            .Subscribe(_ => HandleOff());
    }
    
    private void HandleOff()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
        _schedulerSubscription = _scheduler.Schedule(
            TimeSpan.FromMinutes(TurnBackOnInMinutes), 
            () => _switches.HouseCameraNotificationsNetdaemon.TurnOn());
    }

    private void HandleOn()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
    }
}