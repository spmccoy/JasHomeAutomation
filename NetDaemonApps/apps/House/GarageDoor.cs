using System.Threading.Tasks;
using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class GarageDoor
{
    private readonly CoverEntities _covers;
    private readonly LightEntities _lights;
    private readonly INotificationService _notificationService;

    public GarageDoor(CoverEntities covers, LightEntities lights, INotificationService notificationService, IHouseService houseService)
    {
        _covers = covers;
        _lights = lights;
        _notificationService = notificationService;
        
        covers.Ratgdov25i0a070cDoor.StateChanges()
            .Where(w => w.New?.State == HaState.Open)
            .Subscribe(_ =>
            {
                covers.HouseGarageDoorNetdaemon.OpenCover();
                houseService.DetermineAndSetHouseState();
            });
        
        covers.Ratgdov25i0a070cDoor.StateChanges()
            .Where(w => w.New?.State == HaState.Closed)
            .Subscribe(_ =>
            {
                covers.HouseGarageDoorNetdaemon.CloseCover();
                houseService.DetermineAndSetHouseState();
            });

        covers.HouseGarageDoorNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.Open)
            .Subscribe(_ =>
            {
                if (covers.Ratgdov25i0a070cDoor.State == HaState.Closed)
                {
                    covers.Ratgdov25i0a070cDoor.OpenCover();
                }
            });
        
        covers.HouseGarageDoorNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.Closed)
            .SubscribeAsync(_ => GarageCloseDelayAndWarning());
    }

    private async Task GarageCloseDelayAndWarning()
    {
        // Task for sending notifications
        var notificationTask = Task.Run(async () =>
        {
            for (var i = 0; i < 3; i++)
            {
                _notificationService.Notify(NotifiableDevice.GarageAlexa, tts: "WARNING: Garage door preparing to close");
                await Task.Delay(2000); // Use Task.Delay for asynchronous delay
            }
        });

        // Task for controlling light blinking
        var lightControlTask = Task.Run(async () =>
        {
            for (var i = 0; i < 6; i++)
            {
                _lights.Ratgdov25i0a070cLight.TurnOn();
                await Task.Delay(500); // Asynchronous delay
                _lights.Ratgdov25i0a070cLight.TurnOff();
                await Task.Delay(500); // Asynchronous delay
            }
        });

        // Await both tasks to complete
        await Task.WhenAll(notificationTask, lightControlTask);

        // Close the garage door after completing the above tasks
        _covers.Ratgdov25i0a070cDoor.CloseCover();
    }
}