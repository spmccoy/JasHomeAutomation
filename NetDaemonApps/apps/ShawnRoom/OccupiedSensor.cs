using System.Reactive.Concurrency;
using NetDaemon.Extensions.Scheduler;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class OccupiedSensor
{
    public OccupiedSensor(
        IScheduler scheduler,
        BinarySensorEntities binarySensors,
        IShawnRoomService shawnRoomService)
    {
        scheduler.ScheduleCron("*/1 * * * *", async void () => await shawnRoomService.UpdateOccupancySensorAsync());

        binarySensors.ShawnsroomOccupiedNetdaemon
            .StateChanges()
            .Subscribe(_ =>
            {
                shawnRoomService.DetermineAndSetRoomState();
            });
    }
}