using Integrations.HomeDevices.HomeAssistantGenerated;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.Covers;
using SmartHomeCore.Infrastructure.Common;

namespace SmartHomeCore.Infrastructure.Integrations.HomeDevices.Rooms;

public class Garage(
    INotificationClient notificationClient,
    CoverEntities covers,
    LightEntities lights)
    : IGarage
{
    public async Task CloseDoorAsync()
    {
        if (covers.Ratgdov25i0a070cDoor.State.ToCoverState() == CoverState.Closed)
        {
            return;
        }
        
        var notificationTask = Task.Run(async () =>
        {
            for (var i = 0; i < 3; i++)
            {
                notificationClient.Notify("Garage door is closing");
                
                await Task.Delay(2000);
            }
        });
        
        var lightControlTask = Task.Run(async () =>
        {
            for (var i = 0; i < 6; i++)
            {
                lights.Ratgdov25i0a070cLight.TurnOn();
                await Task.Delay(500); 
                lights.Ratgdov25i0a070cLight.TurnOff();
                await Task.Delay(500); 
            }
        });
        
        await Task.WhenAll(notificationTask, lightControlTask);
        covers.Ratgdov25i0a070cDoor.CloseCover();
    }

    public Task OpenDoorAsync()
    {
        if (covers.Ratgdov25i0a070cDoor.State.ToCoverState() == CoverState.Open)
        {
            return Task.CompletedTask;
        }
        
        covers.Ratgdov25i0a070cDoor.OpenCover();
        return Task.CompletedTask;
    }
}