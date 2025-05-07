using System.Reactive.Concurrency;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MotionSensor
{
    private DateTime? _lastMotionDateTime;
    
    public MotionSensor(
        BinarySensorEntities binarySensors, 
        IScheduler scheduler, 
        SwitchEntities switches,
        IShawnRoomService shawnRoomService)
    {
        binarySensors.ShawnOfficeHueMotionSensorMotion
             .StateChanges()
             .Where(e => e.New?.State == HaState.On && e.Old?.State == HaState.Off)
             .Subscribe(_ =>
             {
                 _lastMotionDateTime = DateTime.Now;
                 switches.ShawnroomStateNetdaemon.TurnOn();
             });
        
        scheduler.Schedule(TimeSpan.FromMinutes(60), () =>
        {
            if (_lastMotionDateTime == null || switches.ShawnroomStateNetdaemon.IsOff())
            {
                return;
            }
            
            var timeDifferenceInSeconds = (DateTime.Now - _lastMotionDateTime.Value).TotalMinutes;

            if (timeDifferenceInSeconds > 60)
            {
                switches.ShawnroomStateNetdaemon.TurnOff();
            }
            else
            {
                shawnRoomService.DetermineAndSetRoomState();
            }
        });
    }
}