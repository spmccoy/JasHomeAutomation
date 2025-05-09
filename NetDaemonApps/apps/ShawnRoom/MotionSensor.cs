using System.Reactive.Concurrency;
using System.Threading.Tasks;
using MqttEntities.ShawnsRoom;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MotionSensor
{
    public MotionSensor(
        BinarySensorEntities binarySensors, 
        IScheduler scheduler, 
        SwitchEntities switches,
        IShawnRoomService shawnRoomService,
        IMqttEntityManager mqttEntityManager,
        LastMotionInShawnsRoomSensor lastMotionSensor)
    {
        binarySensors.ShawnOfficeHueMotionSensorMotion
             .StateChanges()
             .Where(e => e.New?.State == HaState.On && e.Old?.State == HaState.Off)
             .SubscribeAsync(async _ =>
             {
                 switches.ShawnroomStateNetdaemon.TurnOn();
                 await lastMotionSensor.UpdateMotionStateAsync();
             });
    }
}