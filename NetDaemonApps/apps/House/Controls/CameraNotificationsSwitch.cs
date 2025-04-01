using System.Reactive.Concurrency;

namespace NetDaemonApps.apps.House.Controls;

public class CameraNotificationsSwitch(IScheduler scheduler, Entities entities) 
    : MqttSwitch("House", "camera-notifications", "Camera notifications", On)
{
    private const int TurnBackOnInMinutes = 30;
    private IDisposable? _schedulerSubscription;
    
    protected override void HandleOff()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
        _schedulerSubscription = scheduler.Schedule(
            TimeSpan.FromMinutes(TurnBackOnInMinutes), 
            () => entities.Switch.HouseCameraNotificationsNetdaemon.TurnOn());
    }

    protected override void HandleOn()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
    }
}