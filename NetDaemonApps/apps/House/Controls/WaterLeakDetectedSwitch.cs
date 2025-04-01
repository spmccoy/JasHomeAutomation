using System.Reactive.Concurrency;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Controls;

public class WaterLeakDetectedSwitch(INotificationService notificationService, IScheduler scheduler) 
    : MqttSwitch("House", "water-leak-detected", "Water Leak detected", HaCommonState.Off.ToString())
{
    private IDisposable? _schedulerSubscription;
    private const int NotifyEverySeconds = 30;
    
    protected override void HandleOff()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
    }

    protected override void HandleOn()
    {
        Notify();
    }

    private void Notify()
    {
        if (_schedulerSubscription != null)
        {
            return;
        }
        
        notificationService.Notify(notificationService.AllDevices, "⚠️🚰 WATER LEAK DETECTED", "Warning! water leak detected.");
        
        _schedulerSubscription = scheduler.Schedule(
            TimeSpan.FromSeconds(NotifyEverySeconds), 
            Notify);
    }
}