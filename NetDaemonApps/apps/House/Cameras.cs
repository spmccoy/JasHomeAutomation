using System.Reactive.Concurrency;
using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class Cameras
{
    private const int SilenceNotificationsForSeconds = 30;
    
    private readonly INotificationService _notificationService;
    private readonly IScheduler _scheduler;
    private readonly SwitchEntities _switches;

    public Cameras(
        INotificationService notificationService, 
        IScheduler scheduler,
        BinarySensorEntities binarySensors,
        SwitchEntities switches)
    {
        _notificationService = notificationService;
        _scheduler = scheduler;
        _switches = switches;

        BinarySensorEntity[] alertToPerson = 
        [
            binarySensors.DoorbellPerson,
            binarySensors.FrontPerson,
            binarySensors.EastSidePerson,
            binarySensors.WestSidePerson
        ];

        BinarySensorEntity[] alertToVehicle =
        [
            binarySensors.BackVehicle,
            binarySensors.DoorbellVehicle,
            binarySensors.FrontVehicle,
            binarySensors.EastSideVehicle,
            binarySensors.WestSideVehicle,
        ];

        foreach (var alertingCamera in alertToPerson)
        {
            alertingCamera
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ => HandleDetectedPerson());
        }
        
        foreach (var alertingCamera in alertToVehicle)
        {
            alertingCamera
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
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
        if (_schedulerSubscription != null || _switches.HouseCameraNotificationsNetdaemon.State == HaState.Off)
        {
            return;
        }
        
        _notificationService.Notify(_notificationService.AllDevices, text, tts);
        _schedulerSubscription = _scheduler.Schedule(
            TimeSpan.FromSeconds(SilenceNotificationsForSeconds), 
            () => { });
    }
}