using Integrations.HomeDevices.HomeAssistantGenerated;
using SmartHomeCore.Domain.Locks;
using SmartHomeCore.Infrastructure.Common;

namespace SmartHomeCore.Infrastructure.Integrations.HomeDevices.Rooms;

public class MainRoom(LockEntities locks) : IMainRoom
{
    public Task LockFrontDoorAsync()
    {
        if (locks.HomeConnect620ConnectedSmartLock.State.ToLockState() == LockState.Locked)
        {
            return Task.CompletedTask;
        }
        
        locks.HomeConnect620ConnectedSmartLock.Lock();
        
        return Task.CompletedTask;
    }
}