using System.Reactive.Concurrency;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class Cameras
{
    private const int SilenceNotificationsForSeconds = 30;
    
    private readonly Entities _entities;
    private readonly INotificationService _notificationService;
    private readonly IScheduler _scheduler;

    public Cameras(Entities entities, INotificationService notificationService, IScheduler scheduler)
    {
        _entities = entities;
        _notificationService = notificationService;
        _scheduler = scheduler;

        BinarySensorEntity[] alertToPerson = 
        [
            entities.BinarySensor.BackPerson,
            entities.BinarySensor.DoorbellPerson,
            entities.BinarySensor.FrontPerson,
            entities.BinarySensor.EastSidePerson,
            entities.BinarySensor.WestSidePerson
        ];

        BinarySensorEntity[] alertToVehicle =
        [
            entities.BinarySensor.BackVehicle,
            entities.BinarySensor.DoorbellVehicle,
            entities.BinarySensor.FrontVehicle,
            entities.BinarySensor.EastSideVehicle,
            entities.BinarySensor.WestSideVehicle,
        ];

        foreach (var alertingCamera in alertToPerson)
        {
            alertingCamera
                .StateChanges()
                .Where(w => w.New?.State == HaState.On.ToString())
                .Subscribe(_ => HandleDetectedPerson());
        }
        
        foreach (var alertingCamera in alertToVehicle)
        {
            alertingCamera
                .StateChanges()
                .Where(w => w.New?.State == HaState.On.ToString())
                .Subscribe(_ => HandleDetectedVehicle());
        }
    }

    private IDisposable? _schedulerSubscription;

    private void HandleDetectedPerson()
    {
        Notify("💁🏻‍♂️📸 Person detected", "Person detected");
    }
    
    private void HandleDetectedVehicle()
    {
        Notify("🚙️📸 Vehicle detected", "Vehicle detected");
    }

    private void Notify(string text, string tts)
    {
        if (_schedulerSubscription != null || _entities.Switch.HouseCameraNotificationsNetdaemon.State == HaState.Off.ToString())
        {
            return;
        }
        
        _notificationService.Notify(_notificationService.AllDevices, text, tts);
        _schedulerSubscription = _scheduler.Schedule(
            TimeSpan.FromSeconds(SilenceNotificationsForSeconds), 
            () => { });
    }
}