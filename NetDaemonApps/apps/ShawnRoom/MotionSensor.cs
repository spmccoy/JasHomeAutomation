using System.Reactive.Concurrency;
using Domain.Entities;
using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MotionSensor
{
    private const int CycleOnEveryMinutes = 30;
    
    private readonly Entities _entities;
    private readonly IScheduler _scheduler;
    private IDisposable? _schedulerSubscription;

    public MotionSensor(Entities entities, IScheduler scheduler)
    {
        _entities = entities;
        _scheduler = scheduler;

        entities.BinarySensor.ShawnOfficeHueMotionSensorMotion
             .StateChanges()
             .Where(e => e.New?.State == HaState.On && e.Old?.State == HaState.Off)
             .Subscribe(_ => HandleMotion());
    }

    private void HandleMotion()
    {
        Helpers.CancelSchedule(_schedulerSubscription);
        
        _entities.Switch.ShawnOfficeHueMotionSensorMotionSensorEnabled.TurnOff();
        _entities.Switch.ShawnroomStateNetdaemon.TurnOn();
        
        _schedulerSubscription = _scheduler.Schedule(
            TimeSpan.FromMinutes(CycleOnEveryMinutes),
            () => _entities.Switch.ShawnOfficeHueMotionSensorMotionSensorEnabled.TurnOn());
    }
}