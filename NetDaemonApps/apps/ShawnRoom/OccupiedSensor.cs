using System.Reactive.Concurrency;
using MqttEntities.ShawnsRoom;
using NetDaemon.Extensions.Scheduler;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Extensions;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.ShawnRoom;
[NetDaemonApp]
public class OccupiedSensor
{
    private const int ActivityThresholdMinutes = 10;
    private readonly RemoteEntities _remotes;
    private readonly BinarySensorEntities _binarySensors;
    private readonly SensorEntities _sensors;
    private readonly ShawnsRoomOccupiedBinarySensor _occupiedBinarySensor;

    public OccupiedSensor(
        IScheduler scheduler,
        RemoteEntities remotes,
        BinarySensorEntities binarySensors,
        SensorEntities sensors,
        SelectEntities selects,
        ShawnsRoomOccupiedBinarySensor occupiedBinarySensor)
    {
        _remotes = remotes;
        _binarySensors = binarySensors;
        _sensors = sensors;
        _occupiedBinarySensor = occupiedBinarySensor;

        scheduler.ScheduleCron("*/1 * * * *", UpdateOccupancySensor);

        binarySensors.ShawnsroomOccupiedNetdaemon
            .StateChanges()
            .Where(w => w.New?.State == HaState.Off)
            .Subscribe(_ =>
            {
                selects.ShawnroomStateNetdaemon.SelectOff();
            });
    }

    private async void UpdateOccupancySensor()
    {
        var recentMotionDetected = _sensors.ShawnsroomLastMotionNetdaemon.IsRecent(TimeSpan.FromMinutes(ActivityThresholdMinutes)) ?? true;
        var recentDesktopActivity = _sensors.DesktopTcfukdgDesktopTcfukdgLastactive.IsRecent(TimeSpan.FromMinutes(ActivityThresholdMinutes)) ?? true;

        var isRoomOccupied = recentMotionDetected
               || _remotes.ShawnSOfficeTv.IsOn()
               || _binarySensors.ShawnsMacbookProActive.IsOn()
               || recentDesktopActivity;

        await _occupiedBinarySensor.UpdateOccupancyStateAsync(isRoomOccupied);
    }
}