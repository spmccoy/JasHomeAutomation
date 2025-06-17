using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.DoorEntity;

public class DoorLockStateChangedEvent : DomainEvent
{
    public LockState OldState { get; set; }
    
    public LockState NewState { get; set; }
    
    public DoorLockStateChangedEvent(LockState oldState, LockState newState)
    {
        OldState = oldState;
        NewState = newState;
    }
}