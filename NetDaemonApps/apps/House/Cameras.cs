using System.Linq;
using System.Reactive.Concurrency;
using MqttEntities.Models;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class Cameras
{
    private const int SilenceNotificationsForSeconds = 30;
    
    private readonly INotificationService _notificationService;
    private readonly IScheduler _scheduler;
    private readonly SwitchEntities _switches;

    private bool _notificationTimoutEnabled;

    public Cameras(
        INotificationService notificationService, 
        IScheduler scheduler,
        SwitchEntities switches,
        ICameraService cameraService,
        BinarySensorEntities binarySensors,
        IMainRoomService mainRoomService,
        SelectEntities selects)
    {
        _notificationService = notificationService;
        _scheduler = scheduler;
        _switches = switches;

        foreach (var camera in cameraService.GetAllMonitoringForPeople())
        {
            camera.PersonSensorEntity?
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ =>
                {
                    Notify($"💁🏻‍♂️📸 {camera.Name} camera has detected a person", $"{camera.Name} camera has detected a person.", camera);
                });
        }
        
        foreach (var camera in cameraService.GetAllMonitoringForVehicles())
        {
            camera.VehicleSensorEntity?
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ =>
                {
                    Notify($"🚙️📸 {camera.Name} camera has detected a vehicle", $"{camera.Name} camera has detected a vehicle.", camera);
                });
        }

        foreach (var camera in cameraService.GetAllMonitoringForAnimals())
        {
            camera.AnimalSensorEntity?
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ =>
                {
                    Notify($"🐶📸 {camera.Name} camera has detected an animal", $"{camera.Name} camera has detected an animal.", camera);
                });
        }

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
                        selects.MainroomStateSelectNetdaemon.SelectOption(RoomStates.Off.ToString());
                    });
                }
            });
    }

    private void Notify(string text, string tts, Camera camera)
    {
        if (_notificationTimoutEnabled || _switches.HouseCameraNotificationsNetdaemon.State == HaState.Off)
        {
            return;
        }

        var notification = new Notification
        {
            Text = text,
            Tts = tts,
            Devices = _notificationService.GetAllDevices.ToList(),
            Camera = camera
        };
        
        _notificationService.Notify(notification);
        _notificationTimoutEnabled = true;
        _ = _scheduler.Schedule(
            TimeSpan.FromSeconds(SilenceNotificationsForSeconds),
            () =>
            {
                _notificationTimoutEnabled = false;
            });
    }
}