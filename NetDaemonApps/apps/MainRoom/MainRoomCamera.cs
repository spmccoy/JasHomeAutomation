using System.Reactive.Concurrency;
using MqttEntities.Models;
using NetDaemonApps.Extensions;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.MainRoom;

[NetDaemonApp]
public class MainRoomCamera
{
    private IDisposable? _scheduledTask;
    
    public MainRoomCamera(BinarySensorEntities binarySensors, IMainRoomService mainRoomService, SelectEntities selects, IScheduler scheduler)
    {
        binarySensors.MainPerson
            .StateChanges()
            .WhenStateIsFor(s => s?.State == HaState.On, TimeSpan.FromMicroseconds(500), scheduler)
            .Subscribe(_ =>
            {
                mainRoomService.DetermineAndSetRoomState();

                if (selects.HouseStateNetdaemon.State == RoomStates.Sleep.ToString())
                {
                    _scheduledTask?.Dispose();
                    
                    _scheduledTask = scheduler.Schedule(TimeSpan.FromMinutes(20), () =>
                    {
                        selects.MainroomStateSelectNetdaemon.SelectOff();
                    });
                }
            });
    }
}