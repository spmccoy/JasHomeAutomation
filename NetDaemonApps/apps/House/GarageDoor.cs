using System.Threading.Tasks;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class GarageDoor
{
    private readonly CoverEntities _covers;
    private readonly LightEntities _lights;
    private readonly INotificationService _notificationService;

    public GarageDoor(
        CoverEntities covers, 
        LightEntities lights, 
        ButtonEntities buttons,
        INotificationService notificationService,
        IHouseService houseService)
    {
        _covers = covers;
        _lights = lights;
        _notificationService = notificationService;
        
        covers.Ratgdov25i0a070cDoor.StateChanges()
            .Where(w => w.New?.State == HaState.Open)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });
        
        covers.Ratgdov25i0a070cDoor.StateChanges()
            .Where(w => w.New?.State == HaState.Closed)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });

        buttons.HouseGarageDoorOpenNetdaemon.StateChanges()
            .Subscribe(_ => covers.Ratgdov25i0a070cDoor.OpenCover());
        
        buttons.HouseGarageDoorCloseNetdaemon.StateChanges()
            .SubscribeAsync(_ => GarageCloseDelayAndWarning());
    }

    private async Task GarageCloseDelayAndWarning()
    {
        if (_covers.Ratgdov25i0a070cDoor.State == HaState.Closed)
        {
            return;
        }
        
        // Task for sending notifications
        var notificationTask = Task.Run(async () =>
        {
            for (var i = 0; i < 3; i++)
            {
                var notification = new Notification
                {
                    Tts = "Garage door is closing"
                };
                notification.Devices.Add(_notificationService.GarageAlexa);
                
                _notificationService.Notify(notification);
                
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