using SmartHomeCore.Application.Common;
using SmartHomeCore.Infrastructure.Common;

namespace SmartHomeCore.Infrastructure.Integrations.HomeDevices;

public class HomeAutomationClient(IGarage garage, IMainRoom mainRoom) : IHomeAutomationClient
{   
    public async Task CloseGarageAsync() => await garage.CloseDoorAsync();
    public async Task OpenGarageAsync() => await garage.OpenDoorAsync();
    public async Task LockFrontDoorAsync() => await mainRoom.LockFrontDoorAsync();
    public async Task SecureAllEntryPointsAsync()
    {
        await LockFrontDoorAsync();
        await CloseGarageAsync();
    }
}