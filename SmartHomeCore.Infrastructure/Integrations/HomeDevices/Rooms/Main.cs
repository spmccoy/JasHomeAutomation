using Integrations.HomeDevices.HomeAssistantGenerated;
using SmartHomeCore.Infrastructure.Common;

namespace SmartHomeCore.Infrastructure.Integrations.HomeDevices.Rooms;

public class MainRoom(LockEntities locks) : IMainRoom
{
    public Task LockFrontDoorAsync()
    {
        if (locks.HomeConnect620ConnectedSmartLock.State.ToHomeAssistantState() == HomeAssistantState.Locked)
        {
            return Task.CompletedTask;
        }
        
        locks.HomeConnect620ConnectedSmartLock.Lock();
        
        return Task.CompletedTask;
    }
}