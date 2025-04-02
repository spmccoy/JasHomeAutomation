using System.Reactive.Concurrency;

namespace NetDaemonApps.apps.House.Controls;

public class CameraNotificationsSwitch : MqttSwitch
{
    private const int TurnBackOnInMinutes = 30;
    private IDisposable? _schedulerSubscription;
    private readonly IScheduler _scheduler;
    private readonly Entities _entities;

    public CameraNotificationsSwitch(IScheduler scheduler, Entities entities) 
        : base("House", "camera-notifications", "Camera notifications")
    {
        _scheduler = scheduler;
        _entities = entities;

        InitialValue = On;
    }


    protected override void HandleOff()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
        _schedulerSubscription = _scheduler.Schedule(
            TimeSpan.FromMinutes(TurnBackOnInMinutes), 
            () => _entities.Switch.HouseCameraNotificationsNetdaemon.TurnOn());
    }

    protected override void HandleOn()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
    }
}