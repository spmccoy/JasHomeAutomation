using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Controls;

public class WaterLeakDetectedSwitch : MqttSwitch
{
    private IDisposable? _schedulerSubscription;
    private readonly INotificationService _notificationService;
    private readonly IScheduler _scheduler;

    public WaterLeakDetectedSwitch(INotificationService notificationService, IScheduler scheduler) 
        : base("House", "water-leak-detected", "Water Leak detected")
    {
        _notificationService = notificationService;
        _scheduler = scheduler;

        InitialValue = Off;
    }

    private const int NotifyEverySeconds = 30;
    
    protected override void HandleOff()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
    }

    protected override void HandleOn()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
        Notify();
        _schedulerSubscription = _scheduler.SchedulePeriodic(
            TimeSpan.FromSeconds(NotifyEverySeconds),
            Notify);
    }

    private void Notify()
    {
        _notificationService.Notify(_notificationService.AllDevices, "⚠️🚰 WATER LEAK DETECTED", "Warning! water leak detected.");
    }
}