using System.Reactive.Concurrency;
using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class WaterLeakDetectors
{
    private const int NotifyEverySeconds = 30;
    
    private readonly IScheduler _scheduler;
    private readonly INotificationService _notificationService;

    private IDisposable? _schedulerSubscription;
    
    public WaterLeakDetectors(
        ISensorService sensorService, 
        SwitchEntities switches, 
        IScheduler scheduler,
        INotificationService notificationService)
    {
        _scheduler = scheduler;
        _notificationService = notificationService;
        
        foreach (var leakSensor in sensorService.GetAllWaterLeakSensors())
        {
            leakSensor
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ => switches.HouseWaterLeakDetectedNetdaemon.TurnOn());
        }

        switches.HouseWaterLeakDetectedNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.On)
            .Subscribe(_ => HandleOn());
        
        switches.HouseWaterLeakDetectedNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.Off)
            .Subscribe(_ => HandleOff());
    }
    
    private void HandleOff()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
    }

    private void HandleOn()
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