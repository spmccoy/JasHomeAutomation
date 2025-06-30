using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Infrastructure.Common;

namespace SmartHomeCore.Infrastructure.Integrations.HomeDevices;

public class HomeAutomationClient(IGarage garage, IMainRoom mainRoom, IHaContext haContext) : IHomeAutomationClient
{   
    public async Task CloseGarageAsync() => await garage.CloseDoorAsync();
    public async Task OpenGarageAsync() => await garage.OpenDoorAsync();
    public async Task LockFrontDoorAsync() => await mainRoom.LockFrontDoorAsync();
    public async Task SecureAllEntryPointsAsync()
    {
        await LockFrontDoorAsync();
        await CloseGarageAsync();
    }

    public Task ActivateScene(string sceneId)
    {
        var target = new ServiceTarget { EntityIds = [sceneId] };
        haContext.CallService("scene", "turn_on", target);
        return Task.CompletedTask;
    }
}