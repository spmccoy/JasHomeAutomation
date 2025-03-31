using System.Reactive.Concurrency;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Controls;

public class WaterLeakDetectedSwitch(INotificationService notificationService, IScheduler scheduler, Entities entities) 
    : MqttSwitch("House", "water-leak-detected", "Water Leak detected")
{
    protected override void HandleOff()
    {
    }

    protected override void HandleOn()
    {
        Notify();
    }

    private void Notify()
    {
        if (entities.Switch.HouseWaterLeakDetectedNetdaemon.State == HaCommonState.Off.ToString())
        {
            return;
        }
        notificationService.Notify(notificationService.AllDevices, "⚠️🚰 WATER LEAK DETECTED", "Warning, water leak detected.");
        scheduler.Schedule(TimeSpan.FromSeconds(30), Notify);
    }
}

