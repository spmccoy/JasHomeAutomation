using System.Reactive.Concurrency;
using MqttEntities.Models;
using NetDaemonApps.Extensions;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class MainRoomCamera
{
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
                    scheduler.Schedule(TimeSpan.FromMinutes(20), () =>
                    {
                        selects.MainroomStateSelectNetdaemon.SelectOff();
                    });
                }
            });
    }
}