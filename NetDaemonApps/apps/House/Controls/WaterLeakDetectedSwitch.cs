using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Controls;

public class WaterLeakDetectedSwitch(INotificationService notificationService, IScheduler scheduler, Entities entities) 
    : MqttSwitch("House", "water-leak-detected", "Water Leak detected", Off)
{
    private IDisposable? _schedulerSubscription;
    private const int NotifyEverySeconds = 30;
    
    protected override void HandleOff()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
    }

    protected override void HandleOn()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
        Notify();
        _schedulerSubscription = scheduler.SchedulePeriodic(
            TimeSpan.FromSeconds(NotifyEverySeconds),
            Notify);
    }

    private void Notify()
    {
        notificationService.Notify(notificationService.AllDevices, "⚠️🚰 WATER LEAK DETECTED", "Warning! water leak detected.");
    }
}