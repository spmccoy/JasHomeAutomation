using System.Reactive.Concurrency;
using Domain.Entities;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;

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
        BinarySensorEntities binarySensors,
        SwitchEntities switches)
    {
        _notificationService = notificationService;
        _scheduler = scheduler;
        _switches = switches;

        (BinarySensorEntity entity, string name)[] alertToPerson =
        [
            new (binarySensors.DoorbellPerson, "Doorbell"),
            new (binarySensors.FrontPerson, "Front"),
            new (binarySensors.EastSidePerson, "North Side"),
            new (binarySensors.WestSidePerson, "South Side")
        ];

        (BinarySensorEntity entity, string name)[] alertToVehicle =
        [
            new (binarySensors.BackVehicle, "Backyard"),
            new (binarySensors.DoorbellPerson, "Doorbell"),
            new (binarySensors.FrontPerson, "Front"),
            new (binarySensors.EastSidePerson, "North Side"),
            new (binarySensors.WestSidePerson, "South Side")
        ];

        foreach (var (entity, name) in alertToPerson)
        {
            entity
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ =>
                {
                    Notify($"💁🏻‍♂️📸 {name} camera has detected a person", $"{name} camera has detected a person.");
                });
        }
        
        foreach (var (entity, name) in alertToVehicle)
        {
            entity
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ =>
                {
                    Notify($"🚙️📸 {name} camera has detected a vehicle", $"{name} camera has detected a vehicle.");
                });
        }
    }

    private void Notify(string text, string tts)
    {
        if (_notificationTimoutEnabled || _switches.HouseCameraNotificationsNetdaemon.State == HaState.Off)
        {
            return;
        }
        
        _notificationService.Notify(_notificationService.AllDevices, text, tts);
        _notificationTimoutEnabled = true;
        _ = _scheduler.Schedule(
            TimeSpan.FromSeconds(SilenceNotificationsForSeconds),
            () =>
            {
                _notificationTimoutEnabled = false;
            });
    }
}